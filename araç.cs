using System;
using System.Collections.Generic;

class Arac
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public int Yil { get; set; }
    public decimal Fiyat { get; set; }

    public void BilgiYaz()
    {
        Console.WriteLine($"{Marka} {Model} | {Yil} | {Fiyat} TL");
    }
}

class Program
{
    static List<Arac> araclar = new List<Arac>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== ARAÇ SATIŞ SİSTEMİ ===");
            Console.WriteLine("1- Araç Ekle");
            Console.WriteLine("2- Araçları Listele");
            Console.WriteLine("3- Çıkış");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    AracEkle();
                    break;

                case "2":
                    AracListele();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Geçersiz seçim!");
                    break;
