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
TIMES_PER_SEC = 20 # liczba sendów na sekundę
T = 1/(TIMES_PER_SEC + 3)
HOW_LONG_IN_MIN = 3 # przez ile minut ma trwać program

QUEUE_NAME = "main_queue"


def generate(type, minVal, maxVal, host):
    deltaVal = maxVal - minVal
    x = random.random() * deltaVal
    x += minVal
    print(f"{type} generator start")
    while(True):
        try:
            connection_params = pika.ConnectionParameters(host=host)
            connection = pika.BlockingConnection(connection_params)
            channel = connection.channel()
            print("Connected to rabbitmq.")
            break
        except AMQPConnectionError:
            time.sleep(10)
        
    channel.queue_declare(queue=QUEUE_NAME, durable=True, exclusive=False, auto_delete=False)
    for i in range(TIMES_PER_SEC*60*HOW_LONG_IN_MIN):#sendPerSecXSecsInMinXIleMins
        x1 = random.random()
        if type == "bears" and x1 < 0.1:
            x1 = 1
        if random.random() < 0.45:
            x -= x1
        else:
            x += x1         
        if type != "bears":
            for j in range(7):
                x1 = random.random() - 0.5
                x += (x1/10)   
                channel.basic_publish(exchange='', routing_key=QUEUE_NAME, body=f"{type};{j};{x}")
        else :
            for j in range(7):
                x1 = random.random() - 0.5
                if x1 < -0.4:
                    x -= 1
                elif x1 > 0.4:
                    x += 1
                if x < 0 :
                    x = 0
                channel.basic_publish(exchange='', routing_key=QUEUE_NAME, body=f"{type};{j};{x}")
        time.sleep((1/TIMES_PER_SEC)*7)
    connection.close()
    print(f"{type} generator end")

def main():
    print("generator start")
    host = 'rabbitmq.local'
    t1 = threading.Thread(target=generate, args = ("temperature", TEMPERATURE_MIN, TEMPERATURE_MAX, host, ))
    t2 = threading.Thread(target=generate, args = ("wind", WIND_MIN, WIND_MAX, host, ))
    t3 = threading.Thread(target=generate, args = ("pressure", PRESSURE_MIN, PRESSURE_MAX, host, ))
    t4 = threading.Thread(target=generate, args = ("bears", BEARS_MIN, BEARS_MAX, host, ))

    t1.start()
    t2.start()
    t3.start()
    t4.start()

    t1.join()
    t2.join()
    t3.join()
    t4.join()
    print("generator end")

if __name__ == '__main__':
    main()