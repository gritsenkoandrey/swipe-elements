# Swipe Elements Game:
#### Mobile match-3 game built on (ECS + DI) architecture.
# Dependencies:
#### *Unity 6000.0.58f2 URP*.
#### *[Morpeh ECS](https://github.com/scellecs/morpeh)* - ECS Framework.
#### *[Tri Inspector](https://github.com/codewriter-packages/Tri-Inspector)* - Inspector for Morpeh.
#### *[VContainer](https://github.com/hadashiA/VContainer)* - DI Framework.
#### *[Lit Motion](https://github.com/annulusgames/LitMotion)* - Fast and Zero Allocation Tween Library for Unity.
#### *[Newtonsoft Json](https://github.com/applejag/Newtonsoft.Json-for-Unity)* - Newtonsoft Json For Unity.
# Getting Started:
#### 1. Git Clone <https://github.com/gritsenkoandrey/swipe-elements>.
#### 2. Open the scene at *Assets/Scenes/Bootstrap.unity*.
#### 3. Run the game in the editor.
# Game State Machine:
#### 1. Bootstrap State - initialization.
#### 2. Preparer State - services initialization.
#### 3. Level Loading State - game scene loading.
#### 4. Game State - game loop state.
#### 5. Result State - level win or loss state.
#### 6. Restart State - level reload and resource cleanup.
# Tools & Level Generation:
#### Levels are stored and loaded in JSON format. To generate JSON levels, you can use the Tools/Game/Level Tool.
# Gameplay Demo:
[![Watch the game in action](https://img.youtube.com/vi/HKHciPglNAE/hqdefault.jpg)](https://youtube.com/shorts/HKHciPglNAE)