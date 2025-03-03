import sounddevice as sd
import numpy as np
from matplotlib.animation import FuncAnimation
import matplotlib.pyplot as plt
import socket
import json
import collections
#import time

device_list = sd.query_devices()
#print(device_list)

sd.default.device = [1, 6] # Input, Outputデバイス指定

# グローバル変数の初期化
max_freq = 0
max_amp = 0

# 送信側アドレスの設定
# 送信側IP
SrcIP = "127.0.0.1"
# 送信側ポート番号
SrcPort = 11111
# 送信側アドレスをtupleに格納
SrcAddr = (SrcIP, SrcPort)

# 受信側アドレスの設定
# 受信側IP
DstIP = "127.0.0.1"
#DstIP = "172.16.177.98"
# 受信側ポート番号
DstPort = 22024
# 受信側アドレスをtupleに格納
DstAddr = (DstIP, DstPort)

# ソケット作成
udpClntSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# 送信側アドレスでソケットを設定
udpClntSock.bind(SrcAddr)


target_freq = 165.0  # 例として330Hzをターゲット周波数に設定
band_width = 10.0    # 例として10Hzのバンド幅

recent_data = collections.deque(maxlen=3)

def callback(indata, frames, time, status):
    # indata.shape=(n_samples, n_channels)
    global plotdata
    data = indata[::downsample, 0]
    shift = len(data)
    plotdata = np.roll(plotdata, -shift, axis=0)
    plotdata[-shift:] = data


def update_plot(frame):
    """This is called by matplotlib for each plot update.
    """
    global plotdata, window
    x = plotdata[-N:] * window
    F = np.fft.fft(x) # フーリエ変換
    F = F / (N / 2) # フーリエ変換の結果を正規化
    F = F * (N / sum(window)) # 窓関数による補正
    Amp = np.abs(F) # 振幅スペクトル
    line.set_ydata(Amp[:N // 2])

    max_index = np.argmax(Amp[:N // 2])  # 振幅スペクトルの最大値のインデックスを取得
    max_amp = np.max(Amp[:N // 2])
    freqs = np.fft.fftfreq(N, d=1.0/fs)  # 周波数軸を計算
    max_freq = freqs[max_index]
    #print(f"Maximum amplitude frequency: {max_freq} Hz")   振幅スペクトルの最大値とその周波数を表示

    # ターゲット周波数帯域にあるスペクトルの合計強度を計算
    band_sum = (freqs[:N // 2] >= target_freq - band_width / 2) & (freqs[:N // 2] <= target_freq + band_width / 2)
    total_intensity = np.sum(Amp[:N // 2][band_sum])

    #Z値計算
    #mean = np.mean(freqs)
    #std = np.std(freqs)
    #z = ( max_freq - mean) / std

    recent_data.append(max_freq)
    z = np.mean(recent_data)

    # 送信データの作成
    data = {
             "freq": float(max_freq),
             "amp": float(max_amp),
             "z" : float(z),
             "intensity": float(total_intensity),
           }
    
    # JSON型に変換してUDPで送信
    data_bytes = json.dumps(data).encode('utf-8')
    udpClntSock.sendto(data_bytes, DstAddr)
    print(data_bytes)
    #テスト用
    #udpClntSock.sendto( b'test', DstAddr )
    #print('sent')
    #time.sleep(1)
    return line,   


downsample = 1  # FFTするのでダウンサンプリングはしない
length = int(1000 * 44100 / (1000 * downsample))
plotdata = np.zeros((length))
N =44100            # FFT用のサンプル数
fs = 44100            # 音声データのサンプリング周波数
window = np.hanning(N) # 窓関数
freq = np.fft.fftfreq(N, d=1 / fs)  #周波数スケール

fig, ax = plt.subplots()
line, = ax.plot(freq[:N // 2], np.zeros(N // 2))
ax.set_ylim([0, 1])
ax.set_xlim([0, 1500])
ax.set_xlabel('Frequency [Hz]')
ax.set_ylabel('Amplitude spectrum')
fig.tight_layout()

stream = sd.InputStream(
        channels=1,
        dtype='float32',
        callback=callback
    )
ani = FuncAnimation(fig, update_plot, interval=30, blit=True)
with stream:
    plt.show()



# ソケットを閉じる
udpClntSock.close()
