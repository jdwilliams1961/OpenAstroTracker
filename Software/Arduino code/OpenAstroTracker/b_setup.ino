LcdMenu lcdMenu(&lcd, 16);

DayTime RATime;
DayTime RAAdjustedTime;
DayTime HATime;

void setup() {

  //Serial.begin(38400);
  Serial.begin(9600);
  //BT.begin(9600);

  log("Hello World!");
  
  lcd.begin(16, 2);
  lcd.createChar(0, DEG);
  lcd.createChar(1, min);
  lcd.createChar(2, sec);

  stepperRA.setMaxSpeed(RAspeed);
  stepperRA.setAcceleration(RAacceleration);
  stepperDEC.setMaxSpeed(DECspeed);
  stepperDEC.setAcceleration(DECacceleration);
  stepperTRK.setMaxSpeed(10);
  stepperTRK.setAcceleration(2500);
  stepperGUIDE.setMaxSpeed(50);
  stepperGUIDE.setAcceleration(100);

  inputcal = EEPROM.read(0);
  HATime = DayTime(EEPROM.read(1), EEPROM.read(2), 0);
  hourHA = HATime.getHours();
  minHA = HATime.getMinutes();

  logv("HATime = %s  oldHA = %d:%d",HATime.ToString().c_str(), hourHA, minHA);

  lcdMenu.addItem("RAs", RA_Menu);
  lcdMenu.addItem("DEC", DEC_Menu);
  lcdMenu.addItem("HA", HA_Menu);
  if (north) {
    lcdMenu.addItem("POL", Polaris_Menu);
  }
  lcdMenu.addItem("HEAT", Heat_Menu);
  lcdMenu.addItem("CTRL", Control_Menu);
  lcdMenu.addItem("CAL", Calibration_Menu);

  lcdMenu.setActive(RA_Menu);
}