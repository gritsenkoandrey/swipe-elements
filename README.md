# Swipe Elements Game
#### Мобильная match3 игра, построенная на архитектуре (ECS + DI).
# Dependencies:
#### *Unity 6000.0.58f2 URP* | *[Morpeh ECS](https://github.com/scellecs/morpeh)* | *[VContainer](https://vcontainer.hadashikick.jp/)* | *DOTween* | *Newtonsoft.Json*
# Getting Started
#### 1. Git clone <https://github.com/gritsenkoandrey/swipe-elements>.
#### 2. Откройте сцену по пути *Assets/Scenes/Bootstrap.unity*.
#### 3. Запустите игру в редакторе.
# Game State Machine
#### 1. Bootstrap State - инициализация
#### 2. Preparer State - инициализация сервисов
#### 3. Level Loading State - загрузка игровой сцены
#### 4. Game State - состояние игрового цикла
#### 5. Result State - состояние победы или проигрыша на уровне
#### 6. Restart State - перезагрузка уровня и очистка ресурсов
# Tools & Level Generation
#### Уровни хранятся и загружаются в Json формате, для генерации JSON уровней можно воспользоваться утилитой Tools/Serialize Level
# Gameplay Demo
[![Watch the game in action](https://img.youtube.com/vi/HKHciPglNAE/hqdefault.jpg)](https://youtube.com/shorts/HKHciPglNAE)