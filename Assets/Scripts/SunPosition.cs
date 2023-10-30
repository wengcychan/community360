using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SunPosition : MonoBehaviour
{
    private int year;
    private int month;
    private int day;
    private int hour;
    private int minute;
    private int second;
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;
    public TMP_Dropdown dropdown3;
    public Slider slider;
    private int[] selectedIndexs = {0, 0 , 0};
    private float selectedValue = 0;
    public int defaultValueIndex;

    // Latitude and longitude of the location in degrees
    float latitude = 60.19513f;
    float longitude = 24.98001f;

    // UTC offset of the location in hours
    float utcOffset = -2f;

    // Update is called once per frame
    void Start()
    {
        dropdown2.value = defaultValueIndex;
        year = 2023;
        month = 7;
        day = 1;
        hour = 9;
        minute = 0;
        second = 0;

        dropdown1.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown1, 0); });
        dropdown2.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown2, 1); });
        dropdown3.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown3, 2); });

        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(slider); });

        // Get the current date and time in UTC
        DateTime utcTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

        UpdateSunPosition(utcTime);
    }

    void DropdownValueChanged(TMP_Dropdown dropdown, int item)
    {
        // Get the new selected index value
        int newIndex = dropdown.value;

        // Check if the new index is different from the previous index
        if (newIndex != selectedIndexs[item])
        {
            DateTime utcTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

            UpdateSunPosition(utcTime);

            // Update the previous index
            selectedIndexs[item] = newIndex;
        }
    }

    void OnSliderValueChanged(Slider slider)
    {
        // Get the new selected index value
        float newValue = slider.value;

        // Check if the new index is different from the previous index
        if (newValue != selectedValue)
        {
            DateTime utcTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

            UpdateSunPosition(utcTime);

            // Update the previous index
            selectedValue = newValue;
        }
    }

    void UpdateSunPosition(DateTime utcTime)
    {
        // Calculate the Julian date from the UTC time
        double julianDate = CalculateJulianDate(utcTime);

        // Calculate the number of centuries since J2000.0
        double t = (julianDate - 2451545.0) / 36525.0;

        // Calculate the position of the sun using the latitude, longitude, and UTC offset
        double altitude, azimuth;
        CalculateSunPosition(julianDate, t, latitude, longitude, utcOffset, out altitude, out azimuth);

        // Convert altitude and azimuth to radians
        float altitudeRad = (float)Mathf.Deg2Rad * (float)altitude;
        float azimuthRad = (float)Mathf.Deg2Rad * (float)azimuth;

        // Calculate the direction vector of the sun
        Vector3 direction = new Vector3(Mathf.Cos(altitudeRad) * Mathf.Cos(azimuthRad), Mathf.Sin(altitudeRad), Mathf.Cos(altitudeRad) * Mathf.Sin(azimuthRad));

        // Update the position and rotation of the sun game object
        transform.position = direction * 1000f;
        transform.rotation = Quaternion.LookRotation(-direction);
    }

    // Calculate the Julian date from the UTC time
    double CalculateJulianDate(DateTime utcTime)
    {
        return utcTime.ToOADate() + 2415018.5;
    }


    public static void CalculateSunPosition(double julianDate, double t, double latitude, double longitude, double utcOffset, out double altitude, out double azimuth)
    {
        double meanLongitude = 280.460 + 36000.771 * t;
        double meanAnomaly = 357.529 + 35999.050 * t;
        double eclipticLongitude = meanLongitude + 1.915 * Math.Sin(Math.PI / 180.0 * meanAnomaly) + 0.020 * Math.Sin(Math.PI / 180.0 * 2 * meanAnomaly);
        double obliquity = 23.439 - 0.013 * t;
        double rightAscension = 180.0 / Math.PI * Math.Atan2(Math.Cos(Math.PI / 180.0 * obliquity) * Math.Sin(Math.PI / 180.0 * eclipticLongitude), Math.Cos(Math.PI / 180.0 * eclipticLongitude));
        double declination = 180.0 / Math.PI * Math.Asin(Math.Sin(Math.PI / 180.0 * obliquity) * Math.Sin(Math.PI / 180.0 * eclipticLongitude));
        double gmst = 280.46061837 + 360.98564736629 * (julianDate - 2451545.0) + 0.000387933 * Math.Pow(t, 2) - Math.Pow(t, 3) / 38710000.0;
        double lmst = gmst + longitude;
        double utcOffsetHours = utcOffset * 60.0;
        double last = lmst + utcOffsetHours / 4.0 + rightAscension - 180.0;
        double latitudeRad = Math.PI / 180.0 * latitude;
        double declinationRad = Math.PI / 180.0 * declination;
        double altitudeRad = Math.Asin(Math.Sin(latitudeRad) * Math.Sin(declinationRad) + Math.Cos(latitudeRad) * Math.Cos(declinationRad) * Math.Cos(Math.PI / 180.0 * last));
        altitude = 180.0 / Math.PI * altitudeRad;
        double azimuthRad = Math.Atan2(Math.Sin(Math.PI / 180.0 * last), Math.Cos(Math.PI / 180.0 * last) * Math.Sin(latitudeRad) - Math.Tan(declinationRad) * Math.Cos(latitudeRad));
        azimuth = 180.0 / Math.PI * azimuthRad;
    }

    public void Slide(float value)
    {
        hour = Mathf.FloorToInt(value / 3600f);
        if (hour == 24)
            hour = 0;
        minute = Mathf.FloorToInt((value % 3600f) / 60f);
        second = Mathf.FloorToInt(value % 60f);
    }

    public void Now()
    {
        DateTime utcTime = DateTime.UtcNow;
        UpdateSunPosition(utcTime);
    }


    public void Year(int index)
    {
        year = index + 2023;
    }

    public void Month(int index)
    {
        month = index + 1;
        if (month >=4 && month <= 11)
            utcOffset = -3f;
    }

    public void Day(int index)
    {
        day = index + 1;
    }
}

