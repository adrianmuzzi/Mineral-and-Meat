using System;

namespace CustomProject
{
    /// <summary>
    /// Manages the overall game flow, phases, players, and win conditions.
    /// Implements the Singleton pattern to ensure only one instance exists.
    /// </summary>
    public class GameManager
    {
        private static GameManager _gameManager;

        private Map _map;
        private Player _player;
        private AIPlayer _opponent;
        private GamePhase _phase;
        private Player _currentTurn;

        /// <summary>
        /// Maps user input strings to movement vector deltas on the map.
        /// </summary>
        private static readonly Dictionary<string, (int dx, int dy)> _directionMap = new Dictionary<string, (int dx, int dy)>
        {
            { "up", (0, -1) },
            { "down", (0, 1) },
            { "left", (-1, 0) },
            { "right", (1, 0) }
        };

        static GameManager() { }

        /// <summary>
        /// Returns the singleton instance of the GameManager.
        /// </summary>
        public static GameManager GetInstance()
        {
            if (_gameManager == null)
                _gameManager = new GameManager();
            return _gameManager;
        }

        /// <summary>
        /// Initializes the game by setting up the map, player, AI, and game phase.
        /// </summary>
        public void InitialiseGame()
        {
            string[] banner = new string[]
            {
                @"           _                      _                                 _   ",   
                @"          (_)                    | |   ___                         | |  ",
                @" _ __ ___  _ _ __   ___ _ __ __ _| |  ( _ )    _ __ ___   ___  __ _| |_ ",
                @"| '_ ` _ \| | '_ \ / _ \ '__/ _` | |  / _ \/\ | '_ ` _ \ / _ \/ _` | __|",
                @"| | | | | | | | | |  __/ | | (_| | | | (_>  < | | | | | |  __/ (_| | |_ ",
                @"|_| |_| |_|_|_| |_|\___|_|  \__,_|_|  \___/\/ |_| |_| |_|\___|\__,_|\__|",
                @"",
                @"--------------------------- by adrian muzzi -----------------------------"
            };

            foreach (string line in banner)
            {
                Console.WriteLine(line);
            }

            ShowInstructions();

            Console.Write("Press any key to begin, if you dare...");
            Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Hmph. So be it.");
            Console.WriteLine();

            Console.Write("Your name, challenger? ");
            string input = Console.ReadLine() ?? "Player";
            Console.WriteLine();

            _map = new Map();
            _player = new Player(input, _map);
            _opponent = new AIPlayer(_map);
            _currentTurn = _player;
            _phase = GamePhase.Harvest;
        }

        /// <summary>
        /// Displays game instructions and command list to the console.
        /// </summary>
        private void ShowInstructions()
        {
            string[] instructions = new string[]
            {
                @"",
                @"--- INSTRUCTIONS ---",
                @"",
                @"Mineral & Meat is a turn based strategy game.",
                @"Wipe your opponent from the map if you wish to be victorious – or at all.",
                @"",
                @"- Enter 'instructions' to see this help",
                @"- Enter 'skip' to move to the next game phase",
                @"- Enter 'quit' to abandon your conquest",
                @"- Enter 'resources' to display your Resources",
                @"- Enter 'units' to display your Units",
                @"- Enter 'structures' to display your Structures",
                @"- Enter 'costs' to display your Unit and Structure costs",
                @"- Enter 'map' to display your view of the Map",
                @"- Provide movement instructions with 'up', 'down', 'left' and 'right'",
                @"- 'X' represents horizontal coordinates, and 'Y' vertical coordinates",
                @"- Friendly Units and Structures display as Green, enemies as Red",
                @"",
                @"Map Key:",
                @"X: Empty | ?: Unknown",
                @"O: Outfort | A: Academy",
                @"V: Vigilant | C: Crier",
                @"R: Resource",
                @""
            };

            foreach (string line in instructions)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Handles player input and supports global commands like skip, quit, and help.
        /// </summary>
        /// <param name="prompt">The prompt to display before reading input.</param>
        /// <returns>The processed user input.</returns>
        private string GetPlayerInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim().ToLower() ?? "";

            switch (input)
            {
                case "skip":
                    Console.WriteLine("Jumping to next phase...");
                    return "skip";
                case "instructions":
                    ShowInstructions();
                    Console.WriteLine();
                    return GetPlayerInput(prompt);
                case "quit":
                    Console.WriteLine();
                    Console.Write("Typical. Begone.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    return "";
                case "resources":
                    Console.WriteLine();
                    _player.PrintResources();
                    Console.WriteLine();
                    return GetPlayerInput(prompt);
                case "costs":
                    Console.WriteLine();
                    _player.PrintCosts();
                    Console.WriteLine();
                    return GetPlayerInput(prompt);
                case "units":
                    Console.WriteLine();
                    _player.PrintUnits();
                    Console.WriteLine();
                    return GetPlayerInput(prompt);
                case "structures":
                    Console.WriteLine();
                    _player.PrintStructures();
                    Console.WriteLine();
                    return GetPlayerInput(prompt);
                case "map":
                    Console.WriteLine();
                    _player.PrintPlayerMapView();
                    return GetPlayerInput(prompt);
                case "cheatmap":
                    Console.WriteLine();
                    _map.PrintMap();
                    Console.Write("AI: ");
                    _opponent.PrintResources();
                    return GetPlayerInput(prompt);
                default:
                    return input;
            }
        }

        /// <summary>
        /// Executes the current game phase, then transitions to the next.
        /// </summary>
        public void RunCurrentPhase()
        {
            Console.WriteLine($"--- {_currentTurn.Name}'s {_phase} Phase ---{Environment.NewLine}");
            switch (_phase)
            {
                case GamePhase.Harvest:
                    HarvestPhase(_currentTurn);
                    NextGamePhase();
                    break;
                case GamePhase.Construct:
                    ConstructPhase(_currentTurn);
                    NextGamePhase();
                    break;
                case GamePhase.MoveAndAttack:
                    MoveAndAttackPhase(_currentTurn);
                    CheckWinCondition();
                    NextGamePhase();
                    NextTurn();
                    break;
            }
        }

        /// <summary>
        /// Advances the turn to the other player.
        /// </summary>
        private void NextTurn() {
            if (_currentTurn == _player) {
                _currentTurn = _opponent;
                Console.WriteLine("!!! Opponent's turn, such imposition !!!\n");
            } else if (_currentTurn == _opponent) {
                _currentTurn = _player;
                Console.WriteLine("!!! Your turn, inevitibility awaits !!!\n");
            }
        }

        /// <summary>
        /// Advances the current game phase in the GamePhase enum sequence.
        /// </summary>
        private void NextGamePhase() {
            //get all enum values as array
            //ie. values = [GamePhase.Harvest, GamePhase.Construct, GamePhase.MoveAndAttack]
            GamePhase[] values = (GamePhase[])Enum.GetValues(typeof(GamePhase));
            //find index of current phase
            //ie. if _phase == GamePhase.Construct, then index = 1
            int index = Array.IndexOf(values, _phase);
            //increment or wrap around
            _phase = values[(index + 1) % values.Length];
            Console.WriteLine();
        }

        /// <summary>
        /// Checks if either player has lost and ends the game if so.
        /// </summary>
        private void CheckWinCondition()
        {
            if (_player.StructureCount == 0)
            {
                Console.WriteLine("-- DEFEAT --");
                Console.WriteLine("You have no remaining structures. Your opponent has claimed this land.");
                Environment.Exit(0);
            }
            else if (_opponent.StructureCount == 0)
            {
                Console.WriteLine("-- VICTORY --");
                Console.WriteLine("Your opponent has been annihilated. You have won.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Handles the Harvest phase logic for both AI and human players.
        /// </summary>
        /// <param name="p">The player currently taking a turn.</param>
        private void HarvestPhase(Player p) {
            if (p is AIPlayer) {
                _opponent.TakeTurn(_phase);
            } else {
                p.PrintPlayerMapView();
                while (true) {
                    string xInput = GetPlayerInput("Enter Harvest X Coordinate: ");
                    if (xInput == "skip") break;

                    string yInput = GetPlayerInput("Enter Harvest Y Coordinate: ");
                    if (yInput == "skip") break;

                    //validate coordinates are int and are in map bounds
                    if (int.TryParse(xInput, out int x) && int.TryParse(yInput, out int y)) {
                        if (_map.InBounds(x, y)) {
                            if (p.HarvestResource(x, y))
                                break;
                        } else
                            Console.WriteLine("Those coordinates don't exist in this place.");
                    } else
                        Console.WriteLine("Valid coordinates only, human.");
                }
            }
        }

        /// <summary>
        /// Handles the Construct phase for both players, prompting human input or delegating to AI.
        /// </summary>
        /// <param name="p">The player currently taking a turn.</param>
        private void ConstructPhase(Player p) {
            if (p is AIPlayer) {
                _opponent.TakeTurn(_phase);
            } else {
                p.PrintStructures();
                Console.WriteLine();
                p.PrintResources();
                Console.WriteLine();
                p.PrintCosts();
                Console.WriteLine();

                while (true) {
                    string input = GetPlayerInput("Build anew, or from an existing edifice? Structure or Unit? ");
                    if (input == "skip")
                        break;

                    if (input == "structure") { //build a structure
                        Console.WriteLine("And where shall we begin placing the bricks? ");
                        string xInput = GetPlayerInput("X Coordinate: ");
                        if (xInput == "skip")
                            break;
                        string yInput = GetPlayerInput("Y Coordinate: ");
                        if (yInput == "skip")
                            break;

                        //validate integer input, is a visible tile, tile is empty
                        if (int.TryParse(xInput, out int x) && int.TryParse(yInput, out int y)) {
                            if (_map.InBounds(x, y)) {
                                if (!p.VisibleTiles.Contains((x, y)))
                                    Console.WriteLine("That location is shrouded in shadows. We can only build upon what we can see.");
                                else if (!_map.GetTile(x, y).IsEmpty())
                                    Console.WriteLine("We can only build upon barren land. That location is occupied.");
                                else {
                                    input = GetPlayerInput("What new eyesore will burden the landscape, Outfort or Academy? ");
                                    if (input == "skip")
                                        break;
                                    else if (!(input == "outfort" || input == "academy"))
                                        Console.WriteLine("Your minions are limited. You must name a structure they know.");
                                    else {
                                        if (p.ConstructStructure(x, y, input))
                                            break;
                                    }
                                }
                            } else
                                Console.WriteLine("Those coordinates don't exist in this place.");
                        } else
                            Console.WriteLine("Valid coordinates only, human.");
                    } else if (input == "unit") { //build a unit
                        input = GetPlayerInput("Which structure shall manufacture our newest abomination? ");
                        if (input == "skip") break;
                        if (int.TryParse(input, out int i)) {
                            if (i > 0 && i <= p.StructureCount) {
                                if (p.ConstructUnit(i - 1))
                                    break;
                            } else
                                Console.WriteLine("No cursed halls heed your request. Provide a valid number.");
                        } else
                            Console.WriteLine("All are mere numbers in this world. Provide a number human.");
                    } else
                        Console.WriteLine("Specify Structure or Unit");
                }
            }
        }

        /// <summary>
        /// Handles the Move and Attack phase for both players.
        /// </summary>
        /// <param name="p">The player currently taking a turn.</param>
        private void MoveAndAttackPhase(Player p) {
            if (p is AIPlayer) {
                _opponent.TakeTurn(_phase);
            } else {
                p.PrintUnits();
                Console.WriteLine();

                while (true) { //enter movement loop
                    string input = GetPlayerInput("To control one's fate. Which unit to move? ");
                    if (input == "skip")
                        break;

                    //validate input is integer, is in _units bounds
                    if (int.TryParse(input, out int i)) {
                        if (i >= 1 && i <= p.UnitCount) {
                            string direction = GetPlayerInput("In what direction lay their fate? ");
                            if (direction == "skip")
                                break;

                            //accept up down left right (see _directionMap) - map to x,y movement
                            if (_directionMap.TryGetValue(direction, out (int x, int y) delta)) {
                                if (p.MoveUnit(i - 1, delta)) break;
                                else
                                    Console.WriteLine("Mere minions cannot step as you command.");
                            } else
                                Console.WriteLine("A beleaguered soul cannot comprehend that order.");
                        } else
                            Console.WriteLine("Units cannot endure outside the bounds of this hollow reality.");
                    } else
                        Console.WriteLine("Units respond to their designated number, human");
                }
            }
        }
    }
}