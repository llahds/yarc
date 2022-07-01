namespace Api.Common
{
    public static class TextExtensions
    {
        public static bool Contains(this string text, string[] words)
        {
            text = text.ToLower();

            var root = new Node();
            var end = new Node() { Character = '\0' };

            foreach (var word in words.Select(W => W.ToLower()))
            {
                var current = root;

                foreach (var chr in word)
                {
                    if (current.Children.ContainsKey(chr) == false)
                    {
                        current.Children.Add(chr, new Node { Character = chr });
                    }

                    current = current.Children[chr];
                }

                if (current.Children.ContainsKey('\0') == false)
                {
                    current.Children.Add(end.Character, end);
                }
            }

            var searchNodes = new List<Node>();

            for (var i = 0; i < text.Length; i++)
            {
                var chr = text[i];

                for (var n = 0; n < searchNodes.Count; n++)
                {
                    if (searchNodes[n].Children.ContainsKey(chr))
                    {
                        searchNodes[n] = searchNodes[n].Children[chr];

                        if (
                            searchNodes[n].Children.ContainsKey('\0')
                            && (i == text.Length - 1 || char.IsLetterOrDigit(text[i + 1]) == false)
                        )
                        {
                            return true;
                        }
                    }
                    else
                    {
                        searchNodes.RemoveAt(n);
                        n--;
                    }
                }

                if (root.Children.ContainsKey(chr) && (i == 0 || text[i - 1] == ' '))
                {
                    searchNodes.Add(root.Children[chr]);
                }
            }

            return false;
        }

        public class Node
        {
            public char Character { get; set; }
            public Dictionary<char, Node> Children { get; set; } = new Dictionary<char, Node>();
        }
    }
}
