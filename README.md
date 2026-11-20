# jason — RentaCar Express

**Estudiante:** Jason Alexander  
**Matrícula:** 2023-4117  
**Proyecto:** Examen práctico de Programación Básica — Windows Forms (C#)

## 1. Descripción del programa
Aplicación de escritorio en Windows Forms para registrar varios alquileres de vehículos de un cliente, mostrar las líneas de alquiler y calcular el importe final con descuento corporativo, entrega a domicilio e ITBIS.

## 2. Datos de entrada
- Nombre del cliente y condición de cliente corporativo.
- Fecha de salida.
- Nombre del vehículo y tarifa base diaria, digitados manualmente.
- Días de alquiler (de 1 a 30).
- Seguro básico o completo; GPS opcional.
- Entrega en sucursal o a domicilio.

## 3. Proceso
1. Validar que cliente y vehículo tengan nombre y que la tarifa sea numérica y mayor que cero.
2. Calcular tarifa diaria = tarifa base + RD$300 si el seguro es completo + RD$100 si lleva GPS.
3. Calcular cada línea = tarifa diaria × días y agregarla a la lista, acumulando el subtotal.
4. Aplicar descuento del 15 % del subtotal si el cliente es corporativo.
5. Agregar RD$500 una sola vez si elige entrega a domicilio.
6. Calcular base imponible = subtotal − descuento + domicilio.
7. Calcular ITBIS = 18 % de la base y total = base + ITBIS.

## 4. Salida
Lista con columnas **Días, Vehículo, Tarifa y Total**. Resultados de subtotal, descuento, domicilio, ITBIS y total a pagar expresados en RD$.

## 5. Prueba solicitada
- Cliente: Carlos Méndez; corporativo.
- Sedán: tarifa 2200, 3 días, seguro completo. Línea: RD$7,500.00.
- Jeepeta: tarifa 3500, 2 días, seguro básico, GPS. Línea: RD$7,200.00.
- Entrega a domicilio.

**Resultado esperado:** subtotal RD$14,700.00; descuento RD$2,205.00; domicilio RD$500.00; base RD$12,995.00; ITBIS RD$2,339.10; **total RD$15,334.10**.

## 6. Capturas de pantalla
Añadir aquí las capturas reales de la aplicación ejecutándose en Visual Studio, preferiblemente dentro de una carpeta `capturas/`.

Ejemplo:

```markdown
![Programa en ejecución](capturas/programa.png)
```

## Abrir y ejecutar
1. Descomprimir el ZIP en Windows.
2. Abrir `jason.sln` en Visual Studio 2026 con la carga de trabajo **Desarrollo de escritorio con .NET** instalada.
3. Seleccionar el proyecto `jason` y presionar **F5** o **Ctrl+F5**.
4. Introducir los datos de prueba, pulsar **Agregar al alquiler** por cada vehículo y luego **Calcular total**.

> Requiere Windows y .NET 10 para Windows Forms. El proyecto usa una interfaz creada en C# sin archivos `.Designer.cs`.
