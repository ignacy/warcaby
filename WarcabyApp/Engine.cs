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
                //return $"Ply level: {level} FROM({this.fromX},{this.fromY}) => TO({this.toX},{this.toY}), Expected Score ({this.boardAfterMove.CurrentScore()})";
                return $"From ({this.fromX},{this.fromY}) => TO({this.toX},{this.toY})";
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
        private readonly int DEFAULT_DEPTH = 6; // 2 Ply = 1 ruch
        public int Depth { get; }
        public Board StartingBoard { get; set; }

        public Engine(Board board)
        {
            this.Depth = DEFAULT_DEPTH;
            this.StartingBoard = (Board)board.Clone();
        }

        public Ply ScoreMoves()
        {
            var movesWithScores = new Dictionary<int, int[]>();
            var movesTree = new TreeNode(new Ply(0, -1, -1, -1, -1, this.StartingBoard));

            AddToTree(movesTree, 1);

            var root = movesTree.Root();

            return root.Minimax().Value;
        }

        public void AddToTree(TreeNode parent, int depth)
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


        public class TreeNode
        {
            private Ply _value;
            private List<TreeNode> _children = new List<TreeNode>();

            public TreeNode(Ply value)
            {
                this._value = value;
            }

            public TreeNode this[int i]
            {
                get { return _children[i]; }
            }

            public TreeNode Minimax(bool maxLevel = true)
            {
                if (this.Children.Count == 0)
                {
                    return this;
                }

                var ordered = this.Children.OrderBy(node => node.ScoreAlphaBeta(maxLevel));

                if (maxLevel)
                {
                    return ordered.Last();
                }
                else
                {
                    return ordered.First();
                }
            }

            public int ScoreAlphaBeta(bool maxLevel, int alpha = int.MinValue, int beta = int.MaxValue) {
                if (this.Children.Count == 0) {
                    return this.Value.boardAfterMove.CurrentScore();
                }

                if (maxLevel) {
                    foreach (var child in this.Children) {
                        alpha = Math.Max(alpha, child.ScoreAlphaBeta(false, alpha, beta));
                        if (alpha >= beta)
                        {
                            return alpha;
                        }
                    }
                    return alpha;
                } else {
                    foreach (var child in this.Children) {
                        beta = Math.Min(beta, child.ScoreAlphaBeta(false, alpha, beta));
                        if (alpha >= beta)
                        {
                            return beta;
                        }
                    }

                    return beta;
                }

            }

            public int Score(bool maxLevel) {
                if (this.Children.Count == 0) {
                    return this.Value.boardAfterMove.CurrentScore();
                }

                var ordered = this.Children.OrderBy(node => node.Value.boardAfterMove.CurrentScore());

                if (maxLevel)
                {
                    return ordered.Last().Score(false);
                }
                else
                {
                    return ordered.First().Score(true);
                }
            }

            public bool IsRoot()
            {
                return this.Parent == null;
            }

            public TreeNode Parent { get; private set; }

            public TreeNode Root()
            {
                var node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }

            public Ply Value { get { return _value; } }

            public ReadOnlyCollection<TreeNode> Children
            {
                get { return _children.AsReadOnly(); }
            }

            public TreeNode AddChild(Ply value)
            {
                var node = new TreeNode(value) { Parent = this };
                _children.Add(node);
                return node;
            }

            public TreeNode[] AddChildren(params Ply[] values)
            {
                return values.Select(AddChild).ToArray();
            }

            public bool RemoveChild(TreeNode node)
            {
                return _children.Remove(node);
            }

            public void Traverse(Action<Ply> action)
            {
                action(Value);
                foreach (var child in Children)
                    child.Traverse(action);
            }

            public void Print()
            {
                Console.WriteLine($"{Value} Expected Score = {this.Score(true)}");
                foreach (var child in Children)
                    child.Print();
            }

            public override string ToString()
            {
                return Value.ToString();
            }

            public IEnumerable<Ply> Flatten()
            {
                return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
            }
        }
    }
}
