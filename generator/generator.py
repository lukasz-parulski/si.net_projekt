from typing import TextIO
import pika
from pika.exceptions import AMQPConnectionError
import threading
import random
import time

TEMPERATURE_MIN = -50 #w stopniach celcjusz (-35)-(-50)
TEMPERATURE_MAX = -35 #w stopniach celcjusz (-35)-(-50)
WIND_MIN = 14 # w km/h 14-22 
WIND_MAX = 22 # w km/h 14-22 
PRESSURE_MIN = 800 # w hPa +-100
PRESSURE_MAX = 1000 # w hPa +-100
BEARS_MIN = 0 # w misiach polarnych
BEARS_MAX = 6 # w misiach polarnych
TIMES_PER_SEC = 10 # liczba sendów na sekundę
T = 1/(TIMES_PER_SEC + 3)
HOW_LONG_IN_MIN = 3 # przez ile minut ma trwać program

QUEUE_NAME = "main_queue"

NUMBER_OF_SENSORS = 7
NUMBER_OF_GENERATED_DATA = 2 # to będzie razy 7

TEMP = "temperature"
WIND = "wind"
PRES = "pressure"
BEARS = "bears"

HOST = 'localhost'
PORT = 17172

def generate(type, minVal, maxVal, host):
    deltaVal = maxVal - minVal
    x = random.random() * deltaVal
    x += minVal
    if type == BEARS :
        x = int(x)
    print(f"{type} generator start")
    while(True):
        try:
            connection_params = pika.ConnectionParameters(host=host, port=PORT)
            connection = pika.BlockingConnection(connection_params)
            channel = connection.channel()
            print("Connected to rabbitmq.")
            break
        except AMQPConnectionError:
            time.sleep(10)
    
    tempo = 0.5
    channel.queue_declare(queue=QUEUE_NAME, durable=True, exclusive=False, auto_delete=False)
    for i in range(NUMBER_OF_GENERATED_DATA):#sendPerSecXSecsInMinXIleMins
        x1 = random.random()
        if type == BEARS and x1 < 0.1:
            x1 = 1
        elif type == BEARS :
            x1 = 0
        if random.random() < tempo:
            x -= x1
        else:
            x += x1         
        if type != BEARS:
            for j in range(NUMBER_OF_SENSORS):
                x1 = random.random() - 0.5
                x += (x1/10)  
                if x > maxVal:
                    tempo = 0.7
                    x = maxVal 
                elif x < minVal:
                    tempo = 0.3
                    x = minVal 
                channel.basic_publish(exchange='', routing_key=QUEUE_NAME, body=f"{type};{j+1};{x}")
        else :
            for j in range(NUMBER_OF_SENSORS):
                x1 = random.random() - 0.5
                if x1 < -0.4:
                    x -= 1
                elif x1 > 0.4:
                    x += 1
                if x > maxVal:
                    tempo = 0.7
                    x = maxVal 
                elif x < minVal:
                    tempo = 0.3
                    x = minVal 
                channel.basic_publish(exchange='', routing_key=QUEUE_NAME, body=f"{type};{j+1};{x}")
        time.sleep((1/TIMES_PER_SEC)*7)
    connection.close()
    print(f"{type} generator end")

def main():
    print(f"generator start with TIMES_PER_SEC = {TIMES_PER_SEC}")
    host = HOST
    t1 = threading.Thread(target=generate, args = (TEMP, TEMPERATURE_MIN, TEMPERATURE_MAX, host, ))
    t2 = threading.Thread(target=generate, args = (WIND, WIND_MIN, WIND_MAX, host, ))
    t3 = threading.Thread(target=generate, args = (PRES, PRESSURE_MIN, PRESSURE_MAX, host, ))
    t4 = threading.Thread(target=generate, args = (BEARS, BEARS_MIN, BEARS_MAX, host, ))

    t1.start()
    t2.start()
    t3.start()
    t4.start()

    t1.join()
    t2.join()
    t3.join()
    t4.join()
    print("generator end")

def send_one_data():
    print("Wybierz typ:")
    print(f"1) {TEMP}")
    print(f"2) {WIND}")
    print(f"3) {PRES}")
    print(f"4) {BEARS}")
    x = input("=> ")
    if not (x == '1' or x == '2' or x == '3' or x == '4'):
        print("ZLY TYP!!")
        return
    if x =='1':
        x = TEMP
    elif x =='2':
        x = WIND
    elif x =='3':
        x = PRES
    elif x =='4':
        x = BEARS
    y = input("Podaj wartosc do wyslania: ")
    while(True):
        try:
            connection_params = pika.ConnectionParameters(host=HOST, port=PORT)
            connection = pika.BlockingConnection(connection_params)
            channel = connection.channel()
            print("Connected to rabbitmq.")
            break
        except AMQPConnectionError:
            time.sleep(10)

    channel.queue_declare(queue=QUEUE_NAME, durable=True, exclusive=False, auto_delete=False)
    channel.basic_publish(exchange='', routing_key=QUEUE_NAME, body=f"{x};{1};{y}")
    print("wyslano dane")
    connection.close()
    print("Polaczenie zamkniete")

def setup():
    print("Dzien dobry")
    global TEMPERATURE_MIN
    global TEMPERATURE_MAX 
    global WIND_MIN 
    global WIND_MAX
    global PRESSURE_MIN 
    global PRESSURE_MAX
    global BEARS_MIN 
    global BEARS_MAX 
    global TIMES_PER_SEC
    global NUMBER_OF_GENERATED_DATA
    x = input(f"Czy chcesz zmienic domyslne wartosci {TEMP}? 1 = tak, 0 = nie: ")
    if x == '1' or x == 'tak':
        mn = input(f"Podaj min dla {TEMP}. Domyslnie bylo: {TEMPERATURE_MIN}: ")
        mx = input(f"Podaj max dla {TEMP}. Domyslnie bylo: {TEMPERATURE_MAX}: ")
        TEMPERATURE_MIN = int(mn)
        TEMPERATURE_MAX = int(mx)
    x = input(f"Czy chcesz zmienic domyslne wartosci {WIND}? 1 = tak, 0 = nie: ")
    if x == '1' or x == 'tak':
        mn = input(f"Podaj min dla {WIND}. Domyslnie bylo: {WIND_MIN}: ")
        mx = input(f"Podaj max dla {WIND}. Domyslnie bylo: {WIND_MAX}: ")
        WIND_MIN = int(mn)
        WIND_MAX = int(mx)
    x = input(f"Czy chcesz zmienic domyslne wartosci {PRES}? 1 = tak, 0 = nie: ")
    if x == '1' or x == 'tak':
        mn = input(f"Podaj min dla {PRES}. Domyslnie bylo: {PRESSURE_MIN}: ")
        mx = input(f"Podaj max dla {PRES}. Domyslnie bylo: {PRESSURE_MAX}: ")
        PRESSURE_MIN = int(mn)
        PRESSURE_MAX = int(mx)
    x = input(f"Czy chcesz zmienic domyslne wartosci {BEARS}? 1 = tak, 0 = nie: ")
    if x == '1' or x == 'tak':
        mn = input(f"Podaj min dla {BEARS}. Domyslnie bylo: {BEARS_MIN}: ")
        mx = input(f"Podaj max dla {BEARS}. Domyslnie bylo: {BEARS_MAX}: ")
        BEARS_MIN = int(mn)
        BEARS_MAX = int(mx)
    x = input("Ile danych (danego typu) na sekunde : ")
    TIMES_PER_SEC = int(x)
    x = input("Ile danych ma być wygenerowanych dla 1 sensora : ")
    NUMBER_OF_GENERATED_DATA = int(x)


if __name__ == '__main__':
    setup()
    main()
    while(1):
        send_one_data()