import matplotlib.pyplot as plt
import pygame
import numpy as np

# モジュール初期化
pygame.mixer.init(frequency=44100, size=-16, channels=2)  # channels を 2 に変更

# 再生時間を指定
time = 1
# 周波数を指定
Hz1 = 330
Hz2 = 495
Hz3 = 990
Hz4 = 1320

#ド1=[264,528,792,1056,1320,1584,1848]
#ド2,ソ1=[396,792,1188,1584,1980]
#ソ2=[594,1188,1782,2376]

#ミ1=[330,660,990,1320,1650,1980]
#ミ2=[495,990,1485,1980]

#ド1ソ1:792(1オクターブ上のソ),ド1ミ1:1320(2オクターブ上のミ),ソ1ミ1:1980(2オクターブ上のシ)

arr_size = 44100 * time * 2
x = np.linspace(0, arr_size, arr_size)
y1 = np.sin(2 * np.pi * Hz1 / 44100 * x) * 10000
y2 = 0.5 * np.sin(2 * np.pi * Hz2 / 44100 * x) * 10000
y3 = 0.1 * np.sin(2 * np.pi * Hz3 / 44100 * x) * 10000
y4 = 0.05 * np.sin(2 * np.pi * Hz4 / 44100 * x) * 10000

y = y1 + y2
y = y.astype(np.int16)  # 16ビットの整数型に変換
xtime = x / 44100

plt.plot(xtime, y)
plt.xlim(0, 0.1)
plt.show()

# 音声データを作成
# 2チャンネルで、16ビットの整数型でデータを作成する
sound_arr = np.column_stack((y, y)).astype(np.int16)

# 音を再生
sound = pygame.mixer.Sound(buffer=sound_arr.tobytes())
sound.play(loops=-1)

# 音声再生が終わるまで待機
pygame.time.wait(int(time * 300))

# pygame のイベント処理を行う
# pygame.event.get()

while True:
    pass
