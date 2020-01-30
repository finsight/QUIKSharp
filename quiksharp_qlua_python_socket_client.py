# -*- coding: utf-8 -*-
import socket
import json

CRLF = "\r\n\r\n"
host = '127.0.0.1'
port_requests = 34130  
port_callbacks = 34131
sok_requests = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sok_callbacks = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sok_callbacks.connect((host , port_callbacks))
sok_requests.connect((host , port_requests))
request = {"data":"Ping","id":"1","cmd":"ping","t":"0"}
raw_data = json.dumps(request)
sok_requests.sendall((raw_data+CRLF).encode())
while(True):
    response = sok_requests.recv(2048)
    print(response)

# Пример Получение стакана котировок по классу и коду бумаги getQuoteLevel2 - Python socket client
# Функция getQuoteLevel2() принимает 2 параметра: Код класса и Код бумаги, а возвращает таблицу, которая имеет следующие поля:
# bid_count -- Количество котировок покупки (STRING)
# offer_count -- Количество котировок продажи (STRING)
# bid -- Котировки спроса (покупки) (TABLE)
# offer -- Котировки предложений (продажи) (TABLE)
# -- Таблицы «bid» и «offer» имеют следующую структуру:
# price -- Цена покупки / продажи (STRING)
# quantity -- Количество в лотах (STRING)

CRLF = "\r\n\r\n"
host = '127.0.0.1'
port_requests = 34130
port_callbacks = 34131
sok_requests = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sok_callbacks = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sok_callbacks.connect((host , port_callbacks))
sok_requests.connect((host , port_requests))
request = {"data":"SPBFUT|SiH0","id":"1","cmd":"getQuoteLevel2","t":""}
raw_data = json.dumps(request)
sok_requests.sendall((raw_data+CRLF).encode())
data = b""
bufsize = 1024
while(True):
    packet = sok_requests.recv(bufsize)
    data += packet
    if len(packet) < bufsize:
        break
data = json.loads(data.decode('cp1251'))
print(data)

    
