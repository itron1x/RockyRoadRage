using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace SplineMesh {
    /// <summary>
    /// A curved line made of oriented nodes.
    /// Each segment is a cubic Bézier curve connected to spline nodes.
    /// It provides methods to get positions and tangent along the spline, specifying a distance or a ratio, plus the curve length.
    /// The spline and the nodes raise events each time something is changed.
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Spline : MonoBehaviour
    {
        /// <summary>
        /// The spline nodes.
        /// Warning, this collection shouldn't be changed manualy. Use specific methods to add and remove nodes.
        /// It is public only for the user to enter exact values of position and direction in the inspector (and serialization purposes).
        /// </summary>
        public List<SplineNode> nodes = new List<SplineNode>();

        /// <summary>
        /// The generated curves. Should not be changed in any way, use nodes instead.
        /// </summary>
        [HideInInspector] public List<CubicBezierCurve> curves = new List<CubicBezierCurve>();

        /// <summary>
        /// The spline length in world units.
        /// </summary>
        public float Length;

        [SerializeField] private bool isLoop;

        public bool IsLoop
        {
            get { return isLoop; }
            set
            {
                isLoop = value;
                updateLoopBinding();
            }
        }

        /// <summary>
        /// Event raised when the node collection changes
        /// </summary>
        public event ListChangeHandler<SplineNode> NodeListChanged;

        /// <summary>
        /// Event raised when one of the curve changes.
        /// </summary>
        [HideInInspector] public UnityEvent CurveChanged = new UnityEvent();

        /// <summary>
        /// Clear the nodes and curves, then add two default nodes for the reset spline to be visible in editor.
        /// </summary>
        private void Reset()
        {
            nodes.Clear();
            curves.Clear();
            // 0-5
            AddNode(new SplineNode(new Vector3(-0.19f, -0.67f, -0.2332129f),
                new Vector3(-0.19f, -0.67f, -0.2332129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.35f, -0.6f, -0.9832129f),
                new Vector3(-0.35f, -0.6f, -0.9832129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.55f, -0.48f, -2.053213f),
                new Vector3(-0.55f, -0.48f, -2.053213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.72f, -0.42f, -2.713213f),
                new Vector3(-0.72f, -0.42f, -2.713213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.84f, -0.35f, -3.693213f),
                new Vector3(-0.84f, -0.35f, -3.693213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.765f, -0.333f, -4.140213f),
                new Vector3(-0.765f, -0.333f, -4.140213f) + new Vector3(1, 0, 0)));
            // 6-10
            AddNode(new SplineNode(new Vector3(-0.566f, -0.362f, -4.357213f),
                new Vector3(-0.566f, -0.362f, -4.357213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.2249999f, -0.361f, -4.460129f),
                new Vector3(-0.2249999f, -0.361f, -4.460129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(0.347f, -0.253f, -4.311213f),
                new Vector3(0.347f, -0.253f, -4.311213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.242f, 0.158f, -2.937213f),
                new Vector3(1.242f, 0.158f, -2.937213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.728f, 0.31f, -2.604213f),
                new Vector3(1.728f, 0.31f, -2.604213f) + new Vector3(1, 0, 0)));
            // 11-15
            AddNode(new SplineNode(new Vector3(2.295f, 0.481f, -2.602213f),
                new Vector3(2.295f, 0.481f, -2.602213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.693f, 0.562f, -2.857213f),
                new Vector3(2.693f, 0.562f, -2.857213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.964f, 0.614f, -3.257213f),
                new Vector3(2.964f, 0.614f, -3.257213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.946f, 0.65f, -3.777213f),
                new Vector3(2.946f, 0.65f, -3.777213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.697f, 0.729f, -4.138213f),
                new Vector3(2.697f, 0.729f, -4.138213f) + new Vector3(1, 0, 0)));
            // 16-20
            AddNode(new SplineNode(new Vector3(2.283f, 0.838f, -4.272213f),
                new Vector3(2.283f, 0.838f, -4.272213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.688f, 1.044f, -4.077213f),
                new Vector3(1.688f, 1.044f, -4.077213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.42f, 1.264f, -3.628213f),
                new Vector3(1.42f, 1.264f, -3.628213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.34f, 1.459f, -2.992213f),
                new Vector3(1.34f, 1.459f, -2.992213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.61f, 1.546f, -2.412213f),
                new Vector3(1.61f, 1.546f, -2.412213f) + new Vector3(1, 0, 0)));
            //21-25
            AddNode(new SplineNode(new Vector3(2.174f, 1.555f, -2.021213f),
                new Vector3(2.174f, 1.555f, -2.021213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.952f, 1.564f, -1.804213f),
                new Vector3(2.952f, 1.564f, -1.804213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(3.62f, 1.561f, -1.593213f),
                new Vector3(3.62f, 1.561f, -1.593213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(4.199f, 1.514f, -1.073213f),
                new Vector3(4.199f, 1.514f, -1.073213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(4.358f, 1.47f, -0.4152129f),
                new Vector3(4.358f, 1.47f, -0.4152129f) + new Vector3(1, 0, 0)));
            //26-31
            AddNode(new SplineNode(new Vector3(4.261f, 1.409f, 0.1687871f),
                new Vector3(4.261f, 1.409f, 0.1687871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(3.911f, 1.344f, 0.5207871f),
                new Vector3(3.911f, 1.344f, 0.5207871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(3.173f, 1.289f, 0.693787f),
                new Vector3(3.173f, 1.289f, 0.693787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.2f, 1.209f, 0.586787f),
                new Vector3(2.2f, 1.209f, 0.586787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.54f, 1.151f, 0.456787f),
                new Vector3(1.54f, 1.151f, 0.456787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(0.89f, 1.018f, 0.5987871f),
                new Vector3(0.89f, 1.018f, 0.5987871f) + new Vector3(1, 0, 0)));
            //32-37
            AddNode(new SplineNode(new Vector3(0.53f, 0.8469999f, 0.9967871f),
                new Vector3(0.53f, 0.8469999f, 0.9967871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(0.41f, 0.629f, 1.506787f),
                new Vector3(0.41f, 0.629f, 1.506787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(0.64f, 0.513f, 1.986787f),
                new Vector3(0.64f, 0.513f, 1.986787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.08f, 0.4f, 2.246787f),
                new Vector3(1.08f, 0.4f, 2.246787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.62f, 0.283f, 2.216787f),
                new Vector3(1.62f, 0.283f, 2.216787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2f, 0.18f, 1.896787f),
                new Vector3(2f, 0.18f, 1.896787f) + new Vector3(1, 0, 0)));
            //28-43
            AddNode(new SplineNode(new Vector3(2.136f, 0.102f, 1.361871f),
                new Vector3(2.136f, 0.102f, 1.361871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.989f, 0.024f, 0.8408706f),
                new Vector3(1.989f, 0.024f, 0.8408706f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.735f, -0.041f, 0.2268706f),
                new Vector3(1.735f, -0.041f, 0.2268706f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.816f, -0.087f, -0.2402129f),
                new Vector3(1.816f, -0.087f, -0.2402129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.132f, -0.13f, -0.746213f),
                new Vector3(2.132f, -0.13f, -0.746213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.205f, -0.192f, -1.354213f),
                new Vector3(2.205f, -0.192f, -1.354213f) + new Vector3(1, 0, 0)));
            //44-49
            AddNode(new SplineNode(new Vector3(2.008f, -0.286f, -1.820213f),
                new Vector3(2.008f, -0.286f, -1.820213f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.428437f, -0.36f, -1.962433f),
                new Vector3(1.428437f, -0.36f, -1.962433f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.028f, -0.502f, -1.74913f),
                new Vector3(1.028f, -0.502f, -1.74913f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(0.948f, -0.618f, -1.335129f),
                new Vector3(0.948f, -0.618f, -1.335129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(1.216f, -0.7550001f, -0.9481294f),
                new Vector3(1.216f, -0.7550001f, -0.9481294f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.737f, -1.018f, -0.7881294f),
                new Vector3(2.737f, -1.018f, -0.7881294f) + new Vector3(1, 0, 0)));
            //55-55
            AddNode(new SplineNode(new Vector3(3.29f, -1.14f, -0.5432129f),
                new Vector3(3.29f, -1.14f, -0.5432129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(3.69f, -1.27f, 0.1857871f),
                new Vector3(3.69f, -1.27f, 0.1857871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(3.29f, -1.46f, 1.176787f),
                new Vector3(3.29f, -1.46f, 1.176787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(2.23f, -1.74f, 1.436787f),
                new Vector3(2.23f, -1.74f, 1.436787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.9999999f, -2f, -0.5332129f),
                new Vector3(-0.9999999f, -2f, -0.5332129f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-1.63f, -1.91f, -0.4632129f),
                new Vector3(-1.63f, -1.91f, -0.4632129f) + new Vector3(1, 0, 0)));
            //56-59
            AddNode(new SplineNode(new Vector3(-2.13f, -1.82f, -0.03321293f),
                new Vector3(-2.13f, -1.82f, -0.03321293f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-2.23f, -1.71f, 0.6367871f),
                new Vector3(-2.23f, -1.71f, 0.6367871f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-1.94f, -1.62f, 1.256787f),
                new Vector3(-1.94f, -1.62f, 1.256787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-1.28f, -1.45f, 1.636787f),
                new Vector3(-1.28f, -1.45f, 1.636787f) + new Vector3(1, 0, 0)));
            //60-63
            AddNode(new SplineNode(new Vector3(-0.67f, -1.21f, 1.656787f),
                new Vector3(-0.67f, -1.21f, 1.656787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.31f, -1.03f, 1.476787f),
                new Vector3(-0.31f, -1.03f, 1.476787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.06999999f, -0.87f, 0.976787f),
                new Vector3(-0.06999999f, -0.87f, 0.976787f) + new Vector3(1, 0, 0)));
            AddNode(new SplineNode(new Vector3(-0.06999999f, -0.78f, 0.5667871f),
                new Vector3(-0.06999999f, -0.78f, 0.5667871f) + new Vector3(1, 0, 0)));

            RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>() {
                type = ListChangeType.clear
            });
            UpdateAfterCurveChanged();
        }

        private void OnEnable() {
            RefreshCurves();
        }

        public ReadOnlyCollection<CubicBezierCurve> GetCurves() {
            return curves.AsReadOnly();
        }

        private void RaiseNodeListChanged(ListChangedEventArgs<SplineNode> args) {
            if (NodeListChanged != null)
                NodeListChanged.Invoke(this, args);
        }

        private void UpdateAfterCurveChanged() {
            Length = 0;
            foreach (var curve in curves) {
                Length += curve.Length;
            }
            CurveChanged.Invoke();
        }

        /// <summary>
        /// Returns an interpolated sample of the spline, containing all curve data at this time.
        /// Time must be between 0 and the number of nodes.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public CurveSample GetSample(float t) {
            int index = GetNodeIndexForTime(t);
            return curves[index].GetSample(t - index);
        }

        /// <summary>
        /// Returns the curve at the given time.
        /// Time must be between 0 and the number of nodes.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public CubicBezierCurve GetCurve(float t) {
            return curves[GetNodeIndexForTime(t)];
        }

        private int GetNodeIndexForTime(float t) {
            if (t < 0 || t > nodes.Count - 1) {
                throw new ArgumentException(string.Format("Time must be between 0 and last node index ({0}). Given time was {1}.", nodes.Count - 1, t));
            }
            int res = Mathf.FloorToInt(t);
            if (res == nodes.Count - 1)
                res--;
            return res;
        }
		
	/// <summary>
	/// Refreshes the spline's internal list of curves.
	// </summary>
	public void RefreshCurves() {
            curves.Clear();
            for (int i = 0; i < nodes.Count - 1; i++) {
                SplineNode n = nodes[i];
                SplineNode next = nodes[i + 1];

                CubicBezierCurve curve = new CubicBezierCurve(n, next);
                curve.Changed.AddListener(UpdateAfterCurveChanged);
                curves.Add(curve);
            }
            RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>() {
                type = ListChangeType.clear
            });
            UpdateAfterCurveChanged();
        }

        /// <summary>
        /// Returns an interpolated sample of the spline, containing all curve data at this distance.
        /// Distance must be between 0 and the spline length.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public CurveSample GetSampleAtDistance(float d) {
            if (d < 0 || d > Length)
                throw new ArgumentException(string.Format("Distance must be between 0 and spline length ({0}). Given distance was {1}.", Length, d));
            foreach (CubicBezierCurve curve in curves) {
                // test if distance is approximatly equals to curve length, because spline
                // length may be greater than cumulated curve length due to float precision
                if(d > curve.Length && d < curve.Length + 0.0001f) {
                    d = curve.Length;
                }
                if (d > curve.Length) {
                    d -= curve.Length;
                } else {
                    return curve.GetSampleAtDistance(d);
                }
            }
            throw new Exception("Something went wrong with GetSampleAtDistance.");
        }

        /// <summary>
        /// Adds a node at the end of the spline.
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(SplineNode node) {
            nodes.Add(node);
            if (nodes.Count != 1) {
                SplineNode previousNode = nodes[nodes.IndexOf(node) - 1];
                CubicBezierCurve curve = new CubicBezierCurve(previousNode, node);
                curve.Changed.AddListener(UpdateAfterCurveChanged);
                curves.Add(curve);
            }
            RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>() {
                type = ListChangeType.Add,
                newItems = new List<SplineNode>() { node }
            });

            UpdateAfterCurveChanged();
            updateLoopBinding();
        }

        /// <summary>
        /// Insert the given node in the spline at index. Index must be greater than 0 and less than node count.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        public void InsertNode(int index, SplineNode node) {
            if (index == 0)
                throw new Exception("Can't insert a node at index 0");

            SplineNode previousNode = nodes[index - 1];
            SplineNode nextNode = nodes[index];

            nodes.Insert(index, node);

            curves[index - 1].ConnectEnd(node);

            CubicBezierCurve curve = new CubicBezierCurve(node, nextNode);
            curve.Changed.AddListener(UpdateAfterCurveChanged);
            curves.Insert(index, curve);
            RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>() {
                type = ListChangeType.Insert,
                newItems = new List<SplineNode>() { node },
                insertIndex = index
            });
            UpdateAfterCurveChanged();
            updateLoopBinding();
        }

        /// <summary>
        /// Remove the given node from the spline. The given node must exist and the spline must have more than 2 nodes.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(SplineNode node) {
            int index = nodes.IndexOf(node);

            if (nodes.Count <= 2) {
                throw new Exception("Can't remove the node because a spline needs at least 2 nodes.");
            }

            CubicBezierCurve toRemove = index == nodes.Count - 1 ? curves[index - 1] : curves[index];
            if (index != 0 && index != nodes.Count - 1) {
                SplineNode nextNode = nodes[index + 1];
                curves[index - 1].ConnectEnd(nextNode);
            }

            nodes.RemoveAt(index);
            toRemove.Changed.RemoveListener(UpdateAfterCurveChanged);
            curves.Remove(toRemove);

            RaiseNodeListChanged(new ListChangedEventArgs<SplineNode>() {
                type = ListChangeType.Remove,
                removedItems = new List<SplineNode>() { node },
                removeIndex = index
            });
            UpdateAfterCurveChanged();
            updateLoopBinding();
        }

        SplineNode start, end;
        private void updateLoopBinding() {
            if(start != null) {
                start.Changed -= StartNodeChanged;
            }
            if(end != null) {
                end.Changed -= EndNodeChanged;
            }
            if (isLoop) {
                start = nodes[0];
                end = nodes[nodes.Count - 1];
                start.Changed += StartNodeChanged;
                end.Changed += EndNodeChanged;
                StartNodeChanged(null, null);
            } else {
                start = null;
                end = null;
            }
        }

        private void StartNodeChanged(object sender, EventArgs e) {
            end.Changed -= EndNodeChanged;
            end.Position = start.Position;
            end.Direction = start.Direction;
            end.Roll = start.Roll;
            end.Scale = start.Scale;
            end.Up = start.Up;
            end.Changed += EndNodeChanged;
        }

        private void EndNodeChanged(object sender, EventArgs e) {
            start.Changed -= StartNodeChanged;
            start.Position = end.Position;
            start.Direction = end.Direction;
            start.Roll = end.Roll;
            start.Scale = end.Scale;
            start.Up = end.Up;
            start.Changed += StartNodeChanged;
        }

        public CurveSample GetProjectionSample(Vector3 pointToProject) {
            CurveSample closest = default(CurveSample);
            float minSqrDistance = float.MaxValue;
            foreach (var curve in curves) {
                var projection = curve.GetProjectionSample(pointToProject);
                if (curve == curves[0]) {
                    closest = projection;
                    minSqrDistance = (projection.location - pointToProject).sqrMagnitude;
                    continue;
                }
                var sqrDist = (projection.location - pointToProject).sqrMagnitude;
                if (sqrDist < minSqrDistance) {
                    minSqrDistance = sqrDist;
                    closest = projection;
                }
            }
            return closest;
        }
    }

    public enum ListChangeType {
        Add,
        Insert,
        Remove,
        clear,
    }
    public class ListChangedEventArgs<T> : EventArgs {
        public ListChangeType type;
        public List<T> newItems;
        public List<T> removedItems;
        public int insertIndex, removeIndex;
    }
    public delegate void ListChangeHandler<T2>(object sender, ListChangedEventArgs<T2> args);

}
