using System;

namespace CustomProject;

/// <summary>
/// Represents a resource tile on the map that contains quantities of various resources.
/// </summary>
public class Resource : MapObject {
    private Dictionary<ResourceType, int> _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="Resource"/> class.
    /// </summary>
    /// <param name="content">A dictionary containing the types and amounts of resources.</param>
    /// <param name="name">The name of the resource object.</param>
    /// <param name="desc">The description of the resource object.</param>
    public Resource(Dictionary<ResourceType, int> content, string name, string desc) : base(name, desc) {
        _content = content;
    }

    /// <summary>
    /// Gets the content of the resource, mapping resource types to their quantities.
    /// </summary>
    public Dictionary<ResourceType, int> Content => _content;

    /// <summary>
    /// Gets a description of the resource, including its content quantities.
    /// </summary>
    public override string Description {
        get {
            string quantities = "";
            foreach(KeyValuePair<ResourceType, int> i in _content) {
                quantities += $"{i.Key}: {i.Value}";
            }
            return $"{_description} | {quantities}";
        }
    }

    /// <summary>
    /// Displays the resource's representation on the map using cyan text.
    /// </summary>
    public override void PrintSelf() {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("R");
        Console.ForegroundColor = originalColor;
    }
}