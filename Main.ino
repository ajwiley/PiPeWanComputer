#include <DallasTemperature.h>
#include <OneWire.h>

#define ONEWIRE 3

// Temperature sensor variables
OneWire oneWire(ONEWIRE);
DallasTemperature dTemp(&oneWire);

// Flow sensor variables
volatile int NbTopsFan; //measuring the rising edges of the signal
int FlowRate;
int hallsensor = 2;    //The pin location of the sensor
int oldTime = 0;

void rpm () { //Flow sensor calls this interrupt
    NbTopsFan++; // Measure rising and falling edge of sensor
}

void setup() {
  pinMode(hallsensor, INPUT); //initializes digital pin 2 as an input
  attachInterrupt(0, rpm, RISING); //and the interrupt is attached
  
  SerialUSB.begin(9600);
  dTemp.begin();
}

void loop() {
  // Get the temperature from the sensor
  dTemp.requestTemperatures(); 
  NbTopsFan = 0;   //Set NbTops to 0 ready for calculations
  attachInterrupt(2, rpm, FALLING);      //Enables interrupts
  delay (1000);   //Wait 1 second
  
  double Temp = dTemp.getTempFByIndex(0);
  
  detachInterrupt(0);      //Disable interrupts
  FlowRate = ((1000.0 / (millis() - oldTime)) * NbTopsFan) / 5.5; // L/min
  oldTime = millis();

  SerialUSB.print(String(Temp) + "," + String(FlowRate) + "\n");
}
