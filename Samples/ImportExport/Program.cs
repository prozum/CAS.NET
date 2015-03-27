﻿using System;
using System.Collections.Generic;
using ImEx;
using System.Security.Cryptography;

namespace ImportExport
{
    public class Program
    {
        public static void Main()
        {
            // Create and export person
            List<Person> person = new List<Person>();
            person.Add(new Person("Generic", "Twat", 20));
            person.Add(new Person("Average", "joe", 40));
            Export.WriteToCasFile(Export.Serialize(person), "person", "");

            List<Person> person2 = new List<Person>();
            person2.Add(new Person("Generic", "Twat", 25));
            person2.Add(new Person("Average", "joe", 40));
            Export.WriteToCasFile(Export.Serialize(person2), "person2", "");

            /* Test serialize and deserialize objects, and reassign them to their original objects */

            List<Person> culture = new List<Person>();
            culture.Add(new Person("Average", "Joe", 40));
            culture.Add(new ExtendedPerson("Generic", "Twat", 20, "Generic"));
            culture.Add(new Idiot("Total", "Idiot", 25, 42));

            List<TypeManager> liTM = new List<TypeManager>();

            foreach (var item in culture)
            {
                liTM.Add(new TypeManager(item));
            }

            string s = Export.Serialize(liTM);

            List<TypeManager> liTM2 = new List<TypeManager>();

            liTM2 = Import.DeserializeString<List<TypeManager>>(s);
            List<Person> lp = new List<Person>();

            foreach (var item in liTM2)
            {
                // Console.WriteLine(item.s + " " + item.t);
                if (item.t == typeof(Person))
                {
                    //Console.WriteLine(item.s);
                    lp.Add(Import.DeserializeString<Person>(item.s));
                }
                else if (item.t == typeof(ExtendedPerson))
                {
                    //Console.WriteLine(item.s);
                    lp.Add(Import.DeserializeString<ExtendedPerson>(item.s));
                }
                else if (item.t == typeof(Idiot))
                {
                    //Console.WriteLine(item.s);
                    lp.Add(Import.DeserializeString<Idiot>(item.s));
                }
               
            }

            foreach (var item in lp)
            {
                Console.WriteLine(item);
            }

            /*
            // Validate using BSD Checksum
            Console.WriteLine("BSD:");
            string ChecksumString;
            Console.WriteLine("person == person");
            PrintValidation(
                CompareBSDChecksum(
                    Checksum.GetBSDChecksum(
                        Export.Serialize(person)), 
                    Checksum.GetBSDChecksum(
                        Import.ReadSerializedFromCasFile("person", ""))));
            Console.WriteLine("person == person2");
            PrintValidation(
                CompareBSDChecksum(
                    Checksum.GetBSDChecksum(
                        Export.Serialize(person)), 
                    Checksum.GetBSDChecksum(
                        Import.ReadSerializedFromCasFile("person2", ""))));

            Console.WriteLine("\nMD5:");
            Console.WriteLine("person == person");

            // Valudate using MD5 hash
            string S_CSumEx;
            string S_CSumIm;
            bool Valid;

            ChecksumString = Import.ReadSerializedFromCasFile("person", "");
            using (MD5 md5Hash = MD5.Create())
            {
                S_CSumEx = Checksum.GetMd5Hash(Export.Serialize(person));
                S_CSumIm = Checksum.GetMd5Hash(ChecksumString);
                Valid = Checksum.VerifyMd5Hash(S_CSumEx, S_CSumIm);
            }

            PrintChecksums(S_CSumEx, S_CSumIm);
            PrintValidation(Valid);

            Console.WriteLine("person == person2");

            ChecksumString = Import.ReadSerializedFromCasFile("person2", "");
            using (MD5 md5Hash = MD5.Create())
            {
                S_CSumEx = Checksum.GetMd5Hash(Export.Serialize(person));
                S_CSumIm = Checksum.GetMd5Hash(ChecksumString);
                Valid = Checksum.VerifyMd5Hash(S_CSumEx, S_CSumIm);
            }

            PrintChecksums(S_CSumEx, S_CSumIm);
            PrintValidation(Valid);

            // Reading and printing content of files
            List<Person> ReadPerson1 = new List<Person>();
            List<Person> ReadPerson2 = new List<Person>();

            ReadPerson1 = Import.ReadDeserializedFromCasFile<List<Person>>("person", "");
            ReadPerson2 = Import.ReadDeserializedFromCasFile<List<Person>>("person2", "");

            Console.WriteLine("\nContent of files: ");

            PrintPerson("person", ReadPerson1);
            PrintPerson("person2", ReadPerson2);
            /**/
        }
			
        // Returns text based on true or false
        public static void PrintValidation(bool b)
        {
            if (b == true)
            {
                Console.WriteLine("Identical checksum!");
            }
            else
            {
                Console.WriteLine("Warning: The checksum for the file do not match!");
            }
        }

        // Prints checksums
        public static void PrintChecksums(string s1, string s2)
        {
            Console.WriteLine("CSumEx: " + s1);
            Console.WriteLine("CSumIm: " + s2);
        }

        // Prints and compares BSD Checksums
        public static bool CompareBSDChecksum(int CSumEx, int CSumIm)
        {
            Console.WriteLine("CSumEx: " + CSumEx);
            Console.WriteLine("CSumIm: " + CSumIm);

            return CSumEx == CSumIm;
        }

        // Prints persons
        public static void PrintPerson(string s, List<Person> p)
        {
            Console.WriteLine(s);
            foreach (var item in p)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    // Person object used for tests
    public class Person
    {
        public string FirstName;
        public string LastName;
        public int Age;

        public Person()
        {

        }

        public Person(string FirstName, string LastName, int Age)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;
        }

        public override string ToString()
        {
            string s = FirstName + " " + LastName + " " + Age;
            return s;
            ;
        }
    }

    // Person object used for tests
    public class ExtendedPerson : Person
    {
        public string occupation { get; set; }

        public ExtendedPerson(string FirstName, string lastName, int age, string occupation)
        {
            this.FirstName = FirstName;
            this.LastName = lastName;
            this.Age = age;
            this.occupation = occupation;
        }

        public override string ToString()
        {
            string s = FirstName + " " + LastName + " " + Age + " " + occupation;
            return s;
        }
    }

    // Person object used for tests
    public class Idiot : Person
    {
        public int IQ;

        public Idiot(string FirstName, string lastName, int age, int iq)
        {
            this.FirstName = FirstName;
            this.LastName = lastName;
            this.Age = age;
            this.IQ = iq;
        }

        public override string ToString()
        {
            string s = FirstName + " " + LastName + " " + +Age + " " + IQ;
            return s;
        }
    }
}
