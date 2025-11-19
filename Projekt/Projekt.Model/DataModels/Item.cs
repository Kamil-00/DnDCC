namespace Projekt.Model.DataModels;

public class Item
{
    public int Id {get; set;}
    public string Name { get; set; }
    public int Quantity { get; set; } = 1;

    public int CharacterId {get;set;}
    public virtual Character Character {get;set;}
}