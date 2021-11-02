import pika

print("TEST")
connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq.local', port=5672))
channel = connection.channel()

channel.queue_declare(queue='test_queue')

for i in range(10):
    channel.basic_publish(exchange='', routing_key='test_queue', body='Message no ' + str(i+1))
    print(" [x] Sent message no " + str(i+1))
connection.close()