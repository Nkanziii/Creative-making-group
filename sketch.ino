#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>

const uint8_t MPU_ADDR = 0x68;
int16_t ax, ay, az;
bool rightTiltDetected = false;
bool leftTiltDetected = false;
const int RIGHT_THRESHOLD = 8000;
const int LEFT_THRESHOLD  = -8000;
const int CENTER_THRESHOLD = 3000;
void setup()
{
  Wire.begin();
  Serial.begin(9600);
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
  Serial.println("MPU Ready");
}
void loop()
{
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_ADDR, 6, true);
  ax = Wire.read()<<8 | Wire.read();
  ay = Wire.read()<<8 | Wire.read();
  az = Wire.read()<<8 | Wire.read();
  if (ax > RIGHT_THRESHOLD)
  {
    rightTiltDetected = true;
  }
  else if (rightTiltDetected && abs(ax) < CENTER_THRESHOLD)
  {
    Serial.println("ROTATE");
    rightTiltDetected = false;
  }
  
  if (ax < LEFT_THRESHOLD)
  {
    leftTiltDetected = true;
  }
  if (leftTiltDetected && abs(ax) < CENTER_THRESHOLD)
  {
    Serial.println("DROP");
    leftTiltDetected = false;
  }
  //else {Serial.println(String(ax));}
  delay(40);
}
