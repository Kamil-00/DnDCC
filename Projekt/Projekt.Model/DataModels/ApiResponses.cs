using Projekt.Model.DataModels;

namespace Projekt.Model.ApiResponses;

public class DndClassSpellResponse
{
    public List<SpellResults> Spells { get; set; }
}

public class SpellResults
{
    public int Count { get; set; }
    public List<SpellItem> Results { get; set; }
}

public class SpellItem
{
    public string Index { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public string Url { get; set; }
}

public class DndClassProficiencyResponse
{
    public List<ProficiencyChoice> Proficiency_Choices { get; set; }
}

public class ProficiencyChoice
{
    public string Desc { get; set; }
    public int Choose { get; set; }
    public string Type { get; set; }
    public From From { get; set; }
}

public class From
{
    public string Option_Set_Type { get; set; }
    public List<Option> Options { get; set; }
}

public class Option
{
    public string Option_Type { get; set; }
    public ApiItem Item { get; set; }
}

public class Results
{
    public int Count { get; set; }
    public List<ApiItem> ApiItems { get; set; }
}

public class ApiItem
{
    public string Index { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
}

public class ChoiceModel
{
    public string Description { get; set; }
    public int ChooseCount { get; set; }
    public List<OptionModel> Options { get; set; }
}

public class OptionModel
{
    public string Text { get; set; }
    public string Value { get; set; }
}

public class CharacterRequest
{
    public Character Character { get; set; }
    public List<string> Proficiencies { get; set; } = new();
}

public class ItemChoiceModel
{
    public string Description { get; set; }
    public int ChooseCount { get; set; }
    public List<ItemSet> ItemSets { get; set; }

    public void Print()
    {
        Console.WriteLine("Description: " + this.Description);
        int i = 0;
        foreach (var set in this.ItemSets)
        {
            Console.WriteLine("Set no. : " + i);
            foreach (var item in set.Items)
            {
                Console.WriteLine("Item: " + item.Name + " x " + item.Quantity);
            }
            i++;
        }
    }
}

public class ItemSet
{
    public int ChooseCount { get; set; } = 0;
    public List<ItemModel> Items { get; set; }
}

public class ItemModel
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}
