using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDevelopment : MonoBehaviour
{
    public string characterName;
    public Birthday bday;
    public int age;

    private int daysRemaining; 

    // Start is called before the first frame update
    void Start()
    {
        if (bday.month != null && bday.day != 0 && bday.year != 0)
        {
            CalculateAge(bday);
            DaysUntilNextBirthday(bday);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class Birthday
    {
        public string month;
        public int year;
        public int day;
    }

    public void CalculateAge(Birthday birthday)
    {
        // Convert the string month to an integer
        int monthNumber = DateTime.ParseExact(birthday.month, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;

        // Get the current date
        DateTime today = DateTime.Today;

        // Create a DateTime object for the birthday
        DateTime birthDate = new DateTime(birthday.year, monthNumber, birthday.day);

        // Calculate the age
        age = today.Year - birthDate.Year;

        // Adjust the age if the birthday hasn't occurred yet this year
        if (today < birthDate.AddYears(age))
        {
            age--;
        }
    }

    public void DaysUntilNextBirthday(Birthday birthday)
    {
        // Convert the string month to an integer
        int monthNumber = DateTime.ParseExact(birthday.month, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;

        // Get the current date
        DateTime today = DateTime.Today;

        // Create a DateTime object for the next birthday
        DateTime nextBirthday = new DateTime(today.Year, monthNumber, birthday.day);

        // If the next birthday is in the past, set it to the next year
        if (nextBirthday < today)
        {
            nextBirthday = nextBirthday.AddYears(1);
        }

        // Calculate the number of days remaining
        daysRemaining = (nextBirthday - today).Days;
    }
}
