namespace ConsoleMenu
{
    public class Menu
    {
        private readonly Dictionary<string, (string Name, Action action)> _actions = new();
        private readonly bool _caseSensitive;
        private readonly string _exitKey;

        public Menu(bool caseSensitive = false, string exitKey = "E")
        {
            _caseSensitive = caseSensitive;
            _exitKey = exitKey;
        }

        private bool InputEquals(string a, string b)
        {
            return string.Equals(a, b, _caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);
        }

        public bool AddCommand(string shortcut, string name, Action action)
        {
            if (shortcut == null || name == null || action == null)
                return false;
            if (shortcut.Length == 0 || name.Length == 0)
                return false;
            if (_actions.Keys.Any(a => InputEquals(shortcut, a)) || InputEquals(shortcut, _exitKey))
                return false;

            _actions.Add(shortcut, (name, action));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if an action was not found (nothing executed), or an action was invoked; false if exit key was pressed</returns>
        public bool ReadOnce()
        {
            Console.WriteLine();
            foreach(var action in _actions)
            {
                Console.Write($"({(_caseSensitive ? action.Key : action.Key.ToUpper())}}) {action.Value.Name};");
            }
            Console.WriteLine($"{_exitKey} Exit");

            var input = Console.ReadLine();
            while(string.IsNullOrWhiteSpace(input))
            {
                input = Console.ReadLine();
            }

            if (!_actions.ContainsKey(_caseSensitive ? input : input.ToLowerInvariant()))
                return true;
            else if (InputEquals(input, _exitKey))
                return false;
            else
                _actions[_caseSensitive ? input : input.ToLowerInvariant()].action.Invoke();
            return true;
        }

        public bool Read()
        {
            while (ReadOnce())
            {

            }

            return true;
        }
    }
}