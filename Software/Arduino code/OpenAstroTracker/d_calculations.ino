if (RAheat == 1) {
  //tracking = 0;
  //stepperRA.enableOutputs();
  digitalWrite(2, HIGH);
  digitalWrite(3, HIGH);
  digitalWrite(11, HIGH);
  digitalWrite(12, HIGH);
}

if (DECheat == 1) {
  stepperDEC.enableOutputs();
  digitalWrite(15, HIGH);
  digitalWrite(16, HIGH);
  digitalWrite(17, HIGH);
  digitalWrite(18, HIGH);
}


if (tracking == 1) {
  stepperTRK.setSpeed(trackingspeed);
  stepperTRK.runSpeed();
}


currentSecs = millis() / 1000;

RAAdjustedTime.set(RATime);
DayTime HACorrection(HATime);
HACorrection.addTime(-h, -m, -s + sHAcorrection);
RAAdjustedTime.addTime(HACorrection);

// --- old begin
minHAzeit = minHA + Zeit / 60;
if (minHAzeit > 59) {
  minHA -= 60;
  hourHA += 1;
}
if (hourHA > 23) hourHA -= 24;
hourHAzeit = hourHA;

//HA correction
hHAcorrection = hourHAzeit - h;
mHAcorrection = minHAzeit - m;


hourRAprint = hourRA + hHAcorrection;
if (mHAcorrection < 0) hourRAprint -= 1;
minRAprint = minRA + mHAcorrection;
secRAprint = secRA + sHAcorrection;

//if (hourRA > 12) hourRA += -24;

if (minRAprint > 59) {
  minRA -= 60;
  hourRA += 1;
}
if (hourRA > 23) hourRA -= 24;
if (hourRAprint < 0) hourRAprint += 24;
if (minRAprint < 0) minRAprint += 60;

//Serial.println("---------------------------------------");
//Serial.println(format("RA OLD           : %d %d %d",hourRA, minRA, secRA));
//Serial.println(format("RAprint OLD      : %d %d %d",hourRAprint, minRAprint, secRAprint));

// --- old end

if (!north) {
  printdegDEC = degreeDEC - 90;
  if (printdegDEC < -90) degreeDEC = 0;
}
else {
  printdegDEC = degreeDEC + 90;
  if (printdegDEC > 90) degreeDEC = 0;
}