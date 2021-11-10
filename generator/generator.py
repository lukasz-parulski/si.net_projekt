import pika
import threading


def generate(type, minVal, maxVal, queue, host):
    print(f"{type} generator start")
    connection_params = pika.ConnectionParameters(host=host)
    connection = pika.BlockingConnection(connection_params)
    channel = connection.channel()
    channel.queue_declare(queue=queue)
    for i in range(10):
        channel.basic_publish(exchange='', routing_key=queue, body="cosXD")
        print(f"{type} [x] Sent message no " + str(i+1))
    connection.close()
    print(f"{type} generator end")

def main():
    print("generator start")
    host = 'rabbitmq.local'
    t1 = threading.Thread(target=generate, args = ("temperature", -10, 5, "temperature_queue", host, ))
    t2 = threading.Thread(target=generate, args = ("humidity", 10, 15, "humidity_queue", host, ))
    t3 = threading.Thread(target=generate, args = ("pressure", -10, 5, "pressure_queue", host, ))
    t4 = threading.Thread(target=generate, args = ("bears", 10, 15, "bears_queue", host, ))

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