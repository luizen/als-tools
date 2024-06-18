namespace AlsTools.Core.ValueObjects;


public class LiveColor
{
    public LiveColor(int? value, string? name = null, string? hexColor = null)
    {
        Value = value;
        Name = name;
        HexColor = hexColor;
    }

    public LiveColor()
    {
    }

    public int? Value { get; set; }

    public string? Name { get; set; }

    public string? HexColor { get; set; }

    public static LiveColor FromValue(int? value)
    {
        if (!value.HasValue || value < 0)
            return LiveColors.Unset;

        if (value > LiveColors.AllColors.Count)
            return LiveColors.Inexistent;

        return LiveColors.AllColors.TryGetValue(value.Value, out var color) ? color : LiveColors.Unset;
    }

    override public string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}


public static class LiveColors
{
    public const int UnsetValue = -1;

    private const int InexistentValue = 100;

    public static readonly LiveColor Unset = new(UnsetValue);

    public static readonly LiveColor Inexistent = new(InexistentValue);

    public static IDictionary<int, LiveColor> AllColors { get; } = new Dictionary<int, LiveColor>
    {
        { UnsetValue, Unset },
        { 0, new LiveColor(0, "Piggy", "#ff94a6") },
        { 1, new LiveColor(1, "Mango", "#ffa529") },
        { 2, new LiveColor(2, "Retro Vibe", "#cc9926") },
        { 3, new LiveColor(3, "Straw Gold", "#f7f47c") },
        { 4, new LiveColor(4, "Chartreuse", "#bffb00") },
        { 5, new LiveColor(5, "Electric Laser Lime", "##18ff2e") },
        { 6, new LiveColor(6, "Hyperpop Green", "#24fea8") },
        { 7, new LiveColor(7, "Ice Temple", "#5cffe7") },
        { 8, new LiveColor(8, "Clear Sky", "#8ac4ff") },
        { 9, new LiveColor(9, "Pan Purple", "#5480e4") },
        { 10, new LiveColor(10, "Widowmaker", "#92a6ff") },
        { 11, new LiveColor(11, "Pink Orchid", "#d86ce4") },
        { 12, new LiveColor(12, "Schiaparelli Pink", "#e553a0") },
        { 13, new LiveColor(13, "White", "#ffffff") },
        { 14, new LiveColor(14, "Pelati(red)", "#ff3636") },
        { 15, new LiveColor(15, "Apocalyptic Orange", "#f66c03") },
        { 16, new LiveColor(16, "Guinea Pig", "#99724b") },
        { 17, new LiveColor(17, "Pure Sunshine(yellow)", "#fef134") },
        { 18, new LiveColor(18, "Poisonous Dart", "#87ff68") },
        { 19, new LiveColor(19, "Crude Banana(green)", "#3dc203") },
        { 20, new LiveColor(20, "Turquoise", "#02bfaf") },
        { 21, new LiveColor(21, "Brain Freeze", "#18e9ff") },
        { 22, new LiveColor(22, "Fresh Blue of Bel Air ", "#12a3ee") },
        { 23, new LiveColor(23, "Star of Life", "#027dc0") },
        { 24, new LiveColor(24, "Matt Purple", "#876ce4") },
        { 25, new LiveColor(25, "Wisteria", "#b677c6") },
        { 26, new LiveColor(26, "Glamour Pink", "#ff39d3") },
        { 27, new LiveColor(27, "Concrete", "#d0d0d0") },
        { 28, new LiveColor(28, "Flattered Flamingo", "#e2675a") },
        { 29, new LiveColor(29, "Butternut", "#ffa374") },
        { 30, new LiveColor(30, "Gold Digger", "#d3ac71") },
        { 31, new LiveColor(31, "Yellow Chalk", "#edfeae") },
        { 32, new LiveColor(32, "Mermaid Tears", "#d2e398") },
        { 33, new LiveColor(33, "Wasabi", "#bad074") },
        { 34, new LiveColor(34, "Irish Spring", "#9ac48d") },
        { 35, new LiveColor(35, "Mint Coffee", "#d4fde1") },
        { 36, new LiveColor(36, "Water", "#cdf1f8") },
        { 37, new LiveColor(37, "Stardust Evening", "#b9c1e3") },
        { 38, new LiveColor(38, "Viking Diva", "#cdbbe4") },
        { 39, new LiveColor(39, "Dreamy Candy Forest", "#ad98e5") },
        { 40, new LiveColor(40, "Violet Vapor", "#e4dce1") },
        { 41, new LiveColor(41, "Elephant in the Room", "#a9a9a9") },
        { 42, new LiveColor(42, "Oriental Pink", "#c6928b") },
        { 43, new LiveColor(43, "Komodo Dragon", "#b78256") },
        { 44, new LiveColor(44, "Tahini Brown", "#98836a") },
        { 45, new LiveColor(45, "Savannah Grass", "#bfba6a") },
        { 46, new LiveColor(46, "Pea", "#a6be00") },
        { 47, new LiveColor(47, "Lost Golfer", "#7db04d") },
        { 48, new LiveColor(48, "Riverbed", "#87c1ba") },
        { 49, new LiveColor(49, "Lost in Time", "#9cb3c4") },
        { 50, new LiveColor(50, "High Tide", "#84a5c2") },
        { 51, new LiveColor(51, "Easter Egg", "#8393cb") },
        { 52, new LiveColor(52, @"Wizard's Brew", "#a495b5") },
        { 53, new LiveColor(53, "Novel Lilac", "#bf9fbe") },
        { 54, new LiveColor(54, "Pinky Pickle", "#bc7196") },
        { 55, new LiveColor(55, "Gunsmoke", "#7b7b7b") },
        { 56, new LiveColor(56, "Raging Raisin", "#ae3333") },
        { 57, new LiveColor(57, "Garfield", "#a95131") },
        { 58, new LiveColor(58, "Bigfoot", "#724e41") },
        { 59, new LiveColor(59, "Paella", "#dbc300") },
        { 60, new LiveColor(60, "Snakes in the Grass", "#84971e") },
        { 61, new LiveColor(61, "Radiant Foliage", "#539f31") },
        { 62, new LiveColor(62, "Dynasty Green", "#089c8e") },
        { 63, new LiveColor(63, "Blue Velvet", "#226384") },
        { 64, new LiveColor(64, "Smalt", "#1a2e96") },
        { 65, new LiveColor(65, "Blasphemous Blue", "#2f52a2") },
        { 66, new LiveColor(66, "Poppy Pompadour", "#614bad") },
        { 67, new LiveColor(67, "Fuchsia Pheromone", "#a34bad") },
        { 68, new LiveColor(68, "Self-Love", "#cc2e6e") },
        { 69, new LiveColor(69, "Dead Pixel (black)", "#3c3c3c") }
    };
}