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
