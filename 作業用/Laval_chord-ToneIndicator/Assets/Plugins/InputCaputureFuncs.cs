using System;
using System.Text;
using System.Runtime.InteropServices;


namespace lib_audio_analysis
{
    public class InputCaptureFuncs
    {
        [DllImport("lib_audio_analysis", EntryPoint = "create_input_capture", CallingConvention = CallingConvention.StdCall)]
        static extern void create_input_capture(ref IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "delete_input_capture", CallingConvention = CallingConvention.StdCall)]
        static extern void delete_input_capture(ref IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "get_input_devices_list", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern void get_input_devices_list(int index, StringBuilder tmp, IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "get_input_devices_list_size", CallingConvention = CallingConvention.StdCall)]
        static extern int get_input_devices_list_size(IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "init_input_capture", CallingConvention = CallingConvention.StdCall)]
        static extern long init_input_capture(UInt32 sample_rate, UInt16 channels, UInt16 bits_per_sample, Int32 frame_ms, int device_index, IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "get_buf_size", CallingConvention = CallingConvention.StdCall)]
        static extern int get_buf_size(IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "start", CallingConvention = CallingConvention.StdCall)]
        static extern long start(IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "caputre_data", CallingConvention = CallingConvention.StdCall)]
        static extern long caputre_data(ref IntPtr data, ref int capture_length, IntPtr func_object);

        [DllImport("lib_audio_analysis", EntryPoint = "stop", CallingConvention = CallingConvention.StdCall)]
        static extern long stop(IntPtr func_object);

        private IntPtr mInputCap;

        public InputCaptureFuncs()
        {
            mInputCap = new IntPtr();
            create_input_capture(ref mInputCap);
        }

        ~InputCaptureFuncs()
        {
            delete_input_capture(ref mInputCap);
        }

        public long initInputCapture(UInt32 sampleRate, UInt16 channels, UInt16 bitsPerSample, Int32 frameMs, int deviceIndex)
        {
            return init_input_capture(sampleRate, channels, bitsPerSample, frameMs, deviceIndex, mInputCap);
        }

        public long startCapture()
        {
            return start(mInputCap);
        }

        public long getCaptureData(ref IntPtr data, ref int capture_length)
        {
            return caputre_data(ref data, ref capture_length, mInputCap);
        }

        public long stopCapture()
        {
            return stop(mInputCap);
        }

        public int getDataBufferSize()
        {
            return get_buf_size(mInputCap);
        }

        public void getInputDevicesList(int index, StringBuilder tmp)
        {
            get_input_devices_list(index, tmp, mInputCap);
        }

        public int getInputDevicesListSize()
        {
            return get_input_devices_list_size(mInputCap);
        }
    }
}