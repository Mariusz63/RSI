from zeep import Client

wsdl = 'http://localhost:8080/web-service-1.0-SNAPSHOT/HelloWorldImplService?wsdl'

client = Client(wsdl)

response = client.service.getHelloWorldAsString("Python Klient")
print("Odpowiedź:", response)

products = client.service.getProducts()
print("Lista produktów:")
for p in products:
    print(f"- {p.nazwa}: {p.opis}, Cena: {p.cena} zł")