﻿using System;

namespace UKTaxCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Info about program
            PrintWelcomeMessage();
            // Main program execution
            CalculateTax();
        }

        static void CalculateTax()
        {
            // Ask for income
            Console.WriteLine("What is your yearly income? (all numbers no commas or spaces");

            // Store income from user
            double grossIncome = Double.Parse(Console.ReadLine());

            double taxable = CalculateTaxable(grossIncome);
            double taxPercentage = CalculateTaxBands(grossIncome);
            double taxPaid = CalculateIncomeTax(taxable, taxPercentage);
            double weeklyWage = CalculateWeeklyWage(grossIncome);
            double nationalInsurance = CalculateWeeklyNationalInsurance(weeklyWage);


            // Output income
            Console.WriteLine("Total taxable is: " + taxable);
            Console.WriteLine("You fall under tax percentage: " + taxPercentage);
            Console.WriteLine("Tax paid: " + taxPaid);
            Console.WriteLine("Weekly National Insurance: " + nationalInsurance);
            Console.WriteLine("You take home: " + CalculateNetIncome(grossIncome, taxPaid, nationalInsurance*52));

            // Wait for user to end program
            Console.ReadKey();
        }

        static void PrintWelcomeMessage()
        {
            // String interpolation
            string version = "0.0.1";
            string author = "Raj";
            string email = "raj.nry.k@gmail.com";

            // Intro text
            Console.WriteLine("Welcome to my UK tax calculator ");
            Console.WriteLine($"author: {author}, version: {version}, email: {email} \n");
        }

        /**
         * For example, if you earn £1,000 a week, you pay:
         * nothing on the first £162
         * 12% (£87.60) on the next £730
         * 2% (£2.16) on the next £108.
         **/
        static double CalculateWeeklyNationalInsurance(double weeklyWage)
        {
            double nationalInsurance = 0;
            double weeklyAllowance = 162;

            // If our wekly wage is greater than allowance we are eligible for tax
            if(weeklyWage > weeklyAllowance)
            {
                // on the first 730 after (162) tax by 12%
                double taxable = weeklyWage - weeklyAllowance;

                // do we have values fitting in this range
                if(taxable - 730 < 0)
                {
                    // apply 12% tax on next 730
                    nationalInsurance = (taxable) * 0.12;
                }
                else
                {
                    // apply a 2% tax on next 108
                    nationalInsurance += CalculateTwoPercentNI(nationalInsurance, taxable);
                }
            }

            return nationalInsurance;
        }

        static double CalculateTwoPercentNI(double nationalInsurance, double taxable)
        {
            // We have a value greater than 730 so we need to apply two taxes
            nationalInsurance = (730) * 0.12;

            // Apply another tax
            double twoTax = taxable - 730;

            if (twoTax < 108)
            {
                nationalInsurance += (twoTax) * 0.02;
            }
            else
            {
                // Apply the maximum
                nationalInsurance += (108) * 0.02;
            }

            return nationalInsurance;
        }

        static double CalculateIncomeTax(double taxableAmount, double taxPercentage)
        {
            return taxableAmount * taxPercentage;
        }

        static double CalculateWeeklyWage(double grossIncome)
        {
            return grossIncome / 52;
        }

        /**
         * Calculates the amount taxable
         **/
        static double CalculateTaxable(double amount)
        {
            // basic allowance which is not taxed
            double basicPersonalAllowance = 11859;
            double taxable = amount;

            // You don’t get a Personal Allowance on taxable income over £123,700.
            if (amount < 123700)
            {
                taxable = amount - basicPersonalAllowance;
            }

            return taxable;
        }


        /**
         * Calculate tax bands 
         * Personal Allowance  Up to £11,850   0 %
         * Basic rate  £11,851 to £46,350  20 %
         * Higher rate £46,351 to £150,000 40 %
         * Additional rate over £150,000   45 %
         * */
         static double CalculateTaxBands(double grossIncome)
         {
            double percentageTax = 0;

            if(grossIncome >= 11851 && grossIncome <= 46350)
            {
                percentageTax = 0.20;
            }else if(grossIncome >= 46351 && grossIncome <= 150000)
            {
                percentageTax = 0.40;
            }else if(grossIncome > 150000)
            {
                percentageTax = 0.45;
            }

            return percentageTax;
         }

         static double CalculateNetIncome(double grossIncome, double taxPaid, double nationalInsuarance)
         {
            return grossIncome - taxPaid - nationalInsuarance;
         }
    }


}
