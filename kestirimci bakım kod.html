from machine import Pin, PWM, I2C, ADC
import time

# ========= PINLER =========
ENA_PIN = 15
IN1_PIN = 14
IN2_PIN = 13

RPM_PIN = 2
SLOTS_PER_REV = 20

MPU_SDA = 0
MPU_SCL = 1
MPU_ADDR = 0x68

LED_PIN = 18  # tek renk kırmızı LED

ADC_PIN = 26  # ACS712 OUT -> GP26 (ADC0)

# ========= AYARLAR =========
FAN_SPEED = 50000

VIB_TH = 6500
VIB_CLEAR_TH = 5000

RPM_MIN = 200
RPM_JUMP_TH = 600

I_MAX_A = 1.20
I_CLEAR_A = 0.90

CLEAR_STABLE_SEC = 2
BLINK_MS = 50
RETRY_SEC = 6

VREF = 3.3
SENS_V_PER_A = 0.185
ZERO_V = 1.65

# ========= LED =========
led = Pin(LED_PIN, Pin.OUT)

def led_normal():
    led.value(1)

def led_off():
    led.value(0)

def led_alarm_blink(now_ms, last_blink_ms):
    if time.ticks_diff(now_ms, last_blink_ms) >= BLINK_MS:
        led.value(0 if led.value() else 1)
        return now_ms
    return last_blink_ms

# ========= FAN =========
ena = PWM(Pin(ENA_PIN))
ena.freq(1000)
in1 = Pin(IN1_PIN, Pin.OUT)
in2 = Pin(IN2_PIN, Pin.OUT)

def fan_start(speed_u16=FAN_SPEED):
    in1.value(1)
    in2.value(0)
    ena.duty_u16(speed_u16)

def fan_stop():
    ena.duty_u16(0)
    in1.value(0)
    in2.value(0)

# ========= RPM =========
rpm_pin = Pin(RPM_PIN, Pin.IN, Pin.PULL_DOWN)
rpm_last = rpm_pin.value()
rpm_edges = 0
rpm_now = 0
rpm_prev = 0

# ========= MPU =========
i2c = I2C(0, sda=Pin(MPU_SDA), scl=Pin(MPU_SCL), freq=400000)

def read_word(reg):
    hi = i2c.readfrom_mem(MPU_ADDR, reg, 1)[0]
    lo = i2c.readfrom_mem(MPU_ADDR, reg + 1, 1)[0]
    v = (hi << 8) | lo
    if v > 32767:
        v -= 65536
    return v

def read_accel():
    ax = read_word(0x3B)
    ay = read_word(0x3D)
    az = read_word(0x3F)
    return ax, ay, az

time.sleep(0.2)
i2c.writeto_mem(MPU_ADDR, 0x6B, b'\x00')
time.sleep(0.2)

ax0 = ay0 = az0 = 0
for _ in range(10):
    ax, ay, az = read_accel()
    ax0 += ax; ay0 += ay; az0 += az
    time.sleep(0.02)
ax0 //= 10; ay0 //= 10; az0 //= 10

# ========= AKIM =========
adc = ADC(ADC_PIN)

def read_current(samples=10):
    s = 0
    for _ in range(samples):
        s += adc.read_u16()
        time.sleep_us(200)
    raw = s / samples
    v = raw * VREF / 65535
    a = (v - ZERO_V) / SENS_V_PER_A
    return abs(a), v

# ========= MAIN =========
t_last_rpm = time.ticks_ms()
t_last_mpu = t_last_rpm
t_last_print = t_last_rpm
t_last_blink = t_last_rpm
t_last_i = t_last_rpm

alarm = False
stable_ok_sec = 0
vib_delta = 0

cur_a = 0.0
cur_v = 0.0

retry_until_ms = None

fan_start()
led_normal()
print("🚀 Sistem başladı! (Stop / Ctrl+C ile durdur)")

try:
    while True:
        now = time.ticks_ms()

        v = rpm_pin.value()
        if v != rpm_last:
            rpm_edges += 1
            rpm_last = v

        if time.ticks_diff(now, t_last_mpu) >= 100:
            ax, ay, az = read_accel()
            vib_delta = max(abs(ax-ax0), abs(ay-ay0), abs(az-az0))
            t_last_mpu = now

        if time.ticks_diff(now, t_last_i) >= 200:
            cur_a, cur_v = read_current(samples=10)
            t_last_i = now

        rpm_fault = False
        rpm_jump_fault = False
        if time.ticks_diff(now, t_last_rpm) >= 1000:
            rpm_prev = rpm_now
            pulses = (rpm_edges / 2)
            rpm_now = int((pulses * 60) / SLOTS_PER_REV)
            rpm_edges = 0
            t_last_rpm = now

            if rpm_now < RPM_MIN:
                rpm_fault = True
            if abs(rpm_now - rpm_prev) > RPM_JUMP_TH:
                rpm_jump_fault = True

        current_fault = (cur_a > I_MAX_A)
        vib_fault = (vib_delta > VIB_TH)
        fault = vib_fault or rpm_fault or rpm_jump_fault or current_fault

        if (not alarm) and fault:
            alarm = True
            fan_stop()
            stable_ok_sec = 0
            retry_until_ms = time.ticks_add(now, RETRY_SEC * 1000)
            print("\n🚨🚨 ARIZA ALGILANDI!")
            print("🔎 Neden:",
                  "📳 MPU" if vib_fault else "",
                  "🌀 RPM düşük" if rpm_fault else "",
                  "📈 RPM dalga" if rpm_jump_fault else "",
                  "⚡ Akım" if current_fault else "")
            print(f"📊 Akım: {cur_a:.2f}A")

        if alarm:
            t_last_blink = led_alarm_blink(now, t_last_blink)
            if retry_until_ms is not None and time.ticks_diff(now, retry_until_ms) < 0:
                pass
            else:
                ok_vib = (vib_delta < VIB_CLEAR_TH)
                ok_i = (cur_a < I_CLEAR_A)
                if ok_vib and ok_i:
                    if time.ticks_diff(now, t_last_rpm) < 200:
                        stable_ok_sec += 1
                    if stable_ok_sec >= CLEAR_STABLE_SEC:
                        alarm = False
                        retry_until_ms = None
                        fan_start()
                        led_normal()
                        print("\n✅ Normal! Fan tekrar çalışıyor 😎")
                else:
                    stable_ok_sec = 0
                    retry_until_ms = time.ticks_add(now, RETRY_SEC * 1000)
        else:
            led_normal()

        if time.ticks_diff(now, t_last_print) >= 500:
            status = "🚨 ARIZA" if alarm else "✅ NORMAL"
            print(f"{status} | 🌀RPM:{rpm_now:4d} (ön:{rpm_prev:4d}) | 📳VIB:{vib_delta:5d} | ⚡A:{cur_a:0.2f}A | 🔌V:{cur_v:0.2f}V")
            t_last_print = now

except KeyboardInterrupt:
    pass
finally:
    fan_stop()
    led_off()
    print("\n🛑 Sistem durduruldu. Fan ve LED kapalı. 👋")
