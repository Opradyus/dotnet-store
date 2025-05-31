using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

public class Order
{
    [Key]//eğer oluşturduğun classın adı + Id yazarsan sistem bunu direk key value olarak alır.yani temelde OrderId yazdığım için [Key] yazmama gerke yoktu.
    public int Id { get; set; }

    public DateTime SiparisTarihi { get; set; }

    public string AdSoyad { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Sehir { get; set; } = null!;

    public string AdresSatiri { get; set; } = null!;

    public string PostaKodu { get; set; } = null!;

    public string Telefon { get; set; } = null!;

    public string? SiparisNotu { get; set; } = null!;

    public double ToplamFiyat { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

     public double AraToplam()
    {
        return OrderItems.Sum(i => i.Urun.Fiyat * i.Miktar);
    }

    public double Toplam()
    {
        return OrderItems.Sum(i => i.Urun.Fiyat * i.Miktar)*1.2;

    }

}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public Urun Urun { get; set; } = null!;

    public int UrunId { get; set; }

    public double Fiyat { get; set; }

    public int Miktar { get; set; }



}
