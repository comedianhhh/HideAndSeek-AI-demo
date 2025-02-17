# Hide-and-Seek AI Demo 🕵️♂️👾  
*A Unity project demonstrating advanced AI behaviors for NPCs in a dynamic hide-and-seek game environment.*  

[![Unity Version](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)](https://unity.com)  
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)  



## 🤖 AI Systems Overview  
###  🤖 Core Features  
- **Type-Safe FSM**: Generic state system with compile-time safety.  
- **A* Pathfinding**: Dynamic obstacle avoidance and off-mesh navigation.  
- **Behavior-Driven Animation**: Blend trees for NPC locomotion.  
- **Modular AI**: Easily extendable states and behaviors.  
---


## 🧠 Advanced FSM Architecture  
### Key Components  
```csharp
// Strongly-typed base state class
public abstract class FSMBaseState<T> : InternalFSMBaseState where T : FSM {
    protected GameObject owner;
    protected T fsm;

    public override void Init(GameObject _owner, FSM _fsm) {
        owner = _owner;
        fsm = (T)_fsm; // Enforces FSM-type matching
        Debug.Assert(fsm != null, $"{owner.name} requires a {typeof(T)} FSM");
    }
}
```



## 🧠 Why This AI System?
Scalable: Add new states (e.g., "Distract" or "CallForHelp") without refactoring core logic.

Performance:

Uses coroutines for asynchronous path recalculation.

Object pooling for AI agents.

Industry Relevance: Similar to systems used in The Last of Us for enemy AI awareness states.

## 📈 Optimization Tips
Spatial Partitioning: Use grids/quadtrees for efficient line-of-sight checks.

Behavior Weighting: Blend multiple states (e.g., 70% patrol + 30% random wander).

GPU Acceleration: Offload pathfinding to compute shaders for large NPC counts.

## 📸 Media
System	Visual
FSM Workflow	State Diagram
Pathfinding	A* Debug
Animation	Blend Tree
## 🚧 Future Improvements
Add machine learning via Unity ML-Agents for adaptive AI.

Implement sound propagation system for hearing-based detection.

Integrate steering behaviors (e.g., obstacle avoidance flocking).

## 📚 Resources
A* Pathfinding Documentation

Game AI Pro: State Machines
