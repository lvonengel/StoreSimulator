# Store Simulator

Store Simulator is a 3D Unity game where the player runs a store in the city. You can buy inventory, stock shelves, set prices, decorate the space with furniture, and upgrade the store as you earn money. 

This project is still in progress and will continue to expand with new systems and content.

### [View Demo on Personal Portfolio](https://lvonengel.github.io/projects/StoreSimulator)

## Project Overview

My goal for this project was to build a complete store simulator gameplay loop and then layer in features that make the world feel more unique and customizable. The core cycle is stocking items, serving customers, earning profit, and using that profit to unlock more inventory. I also built an optional collectible card system that adds a second progression path, that allows players to open packs, track their cards, and even build decks for a separate card battle experience.


Key Features Include
- Buying inventory and placing items on shelves with a money system and UI
- Buying and moving furniture to decorate and organize the store
- Customer spawning and navigation to stocked shelves
- Checkout queue system where customers purchase items
- Cart based purchasing with delivery fees
- Store level progression that unlocks new stock
- Store expansion upgrades through licenses
- Save system with three profiles that persist money, upgrades, furniture, and inventory
- Collectible card packs that can be sold for profit or opened for collection progress
- Card collection book and custom deck creation for battles


## Core Systems

### Store Simulator Loop
- Inventory purchasing and shelf stocking
- Customer AI flow from entrance to shelves to checkout
- Checkout and payment system with money and UI loop

### Progression and Upgrades
- Store level system that unlocks items over time
- Store space upgrades and licenses to expand playable area
- Furniture purchasing and repositioning for layout design

### Saving and Persistence
- Three save profiles
- Saves data for money, store upgrades, furniture placements, and stock

### Collectible Cards
- Pack opening and card collection tracking
- Card inventory book to view progress
- Multiple pack types and progression tracking
- Custom deck building for a separate battle mode

## Project Structure
- The main folder to look at C# code is: - Assets // Scripts


```plaintext
KitchenChaosMultiplayer/
├── Assets/
│   ├── _Card Assets/
│   │   ├── Art/
│   │   ├── Card Model/
│   │   ├── Font/
│   ├── _Store Simulator Assets/
│   │   ├── Audio/
│   │   ├── Models/
│   ├── Animations/
│   ├── Fonts/
│   ├── Materials/
│   ├── Prefabs/
│   │   ├── Advertisements/
│   │   ├── Cards/
│   │   ├── Customers/
│   │   ├── Furniture/
│   │   ├── Level Basics/
│   │   ├── Stock/
│   ├── Scenes/
│   ├── Scripts/
│   │   ├── Cards/
│   │   │   ├── Battle Cards/
│   │   ├── Info/
│   │   │   ├── Controller/
│   │   ├── Save Data/
│   │   ├── Templates/
│   │   ├── UI/
│   ├── Settings/
├── Packages/
└── README.md