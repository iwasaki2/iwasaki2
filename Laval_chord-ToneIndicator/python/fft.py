import numpy as np
import matplotlib.pyplot as plt

sec = 0.01  # 窓の時間幅 [sec]
f = 422    # 周波数 [Hz]　　　人の声に近い周波数にする
N = 1000  # データ点数　　　　　これが大きすぎると周波数成分がほとんど見えなくなる。（image02の横軸）
A = 1       # 振幅

t = np.linspace(0, sec, N)
y = A * np.sin(2 * np.pi * f * t)

hw = np.hanning(N)
yh = y * hw

plt.plot(t, y)
plt.plot(t, yh)
plt.show()

yf = np.fft.fft(y, N)
yhf = np.fft.fft(yh, N)
xf = np.arange(100, 10100, 100)

plt.plot(xf, np.abs(yf[1:101]))
plt.plot(xf, np.abs(yhf[1:101]))
plt.title("Frequency Spectrum")
plt.xlabel("Frequency [Hz]")
plt.ylabel("Amplitude")
plt.show()