using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WarcabyApp
{
    public class Ply : IComparable
    {
        public int level;
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;
        public int Score { get; }

        public Board boardAfterMove { get; }

        public Ply(int level, int x, int y, int x2, int y2, Board board)
        {
            this.level = level;
            this.fromX = x;
            this.fromY = y;
            this.toX = x2;
            this.toY = y2;
            this.Score = board.CurrentScore();
            this.boardAfterMove = (Board)board.Clone();
        }

        public override string ToString()
        {
            if (this.fromX == -1 && this.fromY == -1)
            {
                return $"Root: {level} Score ({this.boardAfterMove.CurrentScore()})";
            }
            else
            {
                return $"Ply level: {level} FROM({this.fromX},{this.fromY}) => TO({this.toX},{this.toY}), Score ({this.boardAfterMove.CurrentScore()})";
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Ply otherPly = obj as Ply;
            if (otherPly != null)
                return this.Score.CompareTo(otherPly.Score);
            else
                throw new ArgumentException("Object is not a Ply");
        }
    }

    public class Engine
    {
        private readonly int DEFAULT_DEPTH = 3; // 4 Ply = 2 ruchy
        public int Depth { get; }
        public Board StartingBoard { get; set; }

        public Engine(Board board)
        {
            this.Depth = DEFAULT_DEPTH;
            this.StartingBoard = (Board)board.Clone();
        }

        public Dictionary<int, int[]> ScoreMoves()
        {
            var movesWithScores = new Dictionary<int, int[]>();
            var movesTree = new TreeNode<Ply>(new Ply(0, -1, -1, -1, -1, this.StartingBoard));

            AddToTree(movesTree, 1);

            var root = movesTree.Root();

            Console.WriteLine($"ROOT MA {root.Children.Count} dzieci");

            var top = root.Minimax();

            Console.WriteLine(top.Parent.Parent.Parent);
            top.Parent.Parent.Parent.Value.boardAfterMove.PrintToOut();

            Console.WriteLine(top.Parent.Parent);
            top.Parent.Parent.Value.boardAfterMove.PrintToOut();

            Console.WriteLine(top.Parent);
            top.Parent.Value.boardAfterMove.PrintToOut();

            Console.WriteLine(top);
            top.Value.boardAfterMove.PrintToOut();

            return movesWithScores;
        }

        public void AddToTree(TreeNode<Ply> parent, int depth)
        {
            if (depth > this.Depth)
            {
                return;
            }

            foreach (var movesForPawn in parent.Value.boardAfterMove.NextMoves())
            {
                foreach (var move in movesForPawn.Value)
                {
                    var ply = new Ply(depth, movesForPawn.Key.X, movesForPawn.Key.Y, move[0], move[1], new Board(parent.Value.boardAfterMove, movesForPawn.Key, move[0], move[1]));
                    var newChild = parent.AddChild(ply);
                    AddToTree(newChild, depth + 1);
                }
            }
        }


        public class TreeNode<Ply>
        {
            private Ply _value;
            private List<TreeNode<Ply>> _children = new List<TreeNode<Ply>>();

            public TreeNode(Ply value)
            {
                _value = value;
            }

            public TreeNode<Ply> this[int i]
            {
                get { return _children[i]; }
            }

            public TreeNode<Ply> Minimax(bool maxLevel = false)
            {
                if (this.Children.Count == 0)
                {
                    return this;
                }

                if (maxLevel)
                {
                    var max = this.Children.OrderBy(node => node._value).First();
                    return max.Minimax(false);
                }
                else
                {
                    var min = this.Children.OrderByDescending(node => node._value).First();
                    return min.Minimax(true);
                }
            }

            public bool IsRoot()
            {
                return this.Parent == null;
            }

            public TreeNode<Ply> Parent { get; private set; }

            public TreeNode<Ply> Root()
            {
                var node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }

            public Ply Value { get { return _value; } }

            public ReadOnlyCollection<TreeNode<Ply>> Children
            {
                get { return _children.AsReadOnly(); }
            }

            public TreeNode<Ply> AddChild(Ply value)
            {
                var node = new TreeNode<Ply>(value) { Parent = this };
                _children.Add(node);
                return node;
            }

            public TreeNode<Ply>[] AddChildren(params Ply[] values)
            {
                return values.Select(AddChild).ToArray();
            }

            public bool RemoveChild(TreeNode<Ply> node)
            {
                return _children.Remove(node);
            }

            public void Traverse(Action<Ply> action)
            {
                action(Value);
                foreach (var child in _children)
                    child.Traverse(action);
            }

            public void Print()
            {
                Console.WriteLine(_value);
                foreach (var child in _children)
                    child.Print();
            }

            public override string ToString()
            {
                return _value.ToString();
            }

            public IEnumerable<Ply> Flatten()
            {
                return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
            }
        }
    }
}