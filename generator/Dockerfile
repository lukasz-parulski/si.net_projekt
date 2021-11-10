FROM python:3
WORKDIR /usr/src/app

ENV PYTHONUNBUFFERED=1

COPY generator/generator.py ./
RUN pip install pika

CMD ["python", "/usr/src/app/generator.py"]