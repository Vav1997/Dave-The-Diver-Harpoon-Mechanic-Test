# Dave the Diver – Harpoon Mechanic Recreation (Unity)

A Unity prototype recreating the core **harpoon mechanic** from *Dave the Diver*, focused on clean and scalable gameplay architecture rather than just feature implementation.

The project emphasizes modular system design where gameplay, weapon logic, camera behavior, and visual feedback are separated into independent components and connected through events and clear responsibilities.

![Harpoon Gameplay](dave%20the%20diver%20small%20video.gif)

## Architecture & Design

* **Finite State Machine (FSM)** for structured player states (movement, aim, shoot, recovery)
* **Event-driven communication** to decouple Player, Weapon, Camera, and Gameplay systems
* **Composition over Inheritance** with focused components (Controller, Visual, Weapon, etc.)
* **Clear separation of concerns** between gameplay logic, presentation, and input
* **Service-style managers** for global systems (camera, gameplay flow, input handling)
* **Coroutine-based sequencing** for timed gameplay flows and transitions

## Gameplay Features

* Harpoon aiming, shooting, rope simulation, and return behavior
* Fish hit detection and catch/lose flow
* Cinemachine-driven dynamic camera transitions
* Context-based gameplay states with responsive feedback

**Technologies:** Unity • C# • Cinemachine • Unity Input System • FSM • Event-Driven Architecture • Component-Based Design
