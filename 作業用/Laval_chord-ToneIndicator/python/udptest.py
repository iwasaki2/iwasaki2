import socket
import json
import time

# 送信側アドレスの設定
# 送信側IP
SrcIP = "172.16.177.244"
# 送信側ポート番号
SrcPort = 11111
# 送信側アドレスをtupleに格納
SrcAddr = (SrcIP, SrcPort)

# 受信側アドレスの設定
# 受信側IP
#DstIP = "192.168.155.107"
DstIP = "192.168.155.107"
# 受信側ポート番号
DstPort = 22024
# 受信側アドレスをtupleに格納
DstAddr = (DstIP, DstPort)

# ソケット作成
udpClntSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# 送信側アドレスでソケットを設定
udpClntSock.bind(SrcAddr)



#b'{ "freq":440.0, "amp":100.0 }'

while True:
    lis=[220.0, 237.0 , 244.0 , 298.0 , 330.0 , 356.0 , 371.0 , 408.0 , 413.0 , 462.0]
    for i in lis:
        freq=i
        sendData = {
             "freq": float(freq),
             "amp": float(0.01131565597300956),
             "z" : float(0.07973731776266072)
             }
        data_bytes = json.dumps(sendData).encode('utf-8')
        udpClntSock.sendto( data_bytes, DstAddr )
        print(data_bytes)
        time.sleep(1)
