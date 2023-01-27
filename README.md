# Présentation générale 
Notre jeu se nomme Medieval Quest et c’est un Hack'n'slash se déroulant dans un univers médiéval fantastique. Le but du jeu est de survivre face à 10 vagues d’ennemis. Au début du jeu on nous propose un choix entre trois personnages jouables. L’arène du jeu est générée aléatoirement.
# Thème 
On est parti sur un thème médiéval/fantasy au vu des assets de GUI qu’on a choisis parmi ceux proposés. De là, on a trouvé des assets pour l’arène, les personnages et ennemis qui restaient dans le thème.
# Implémentation 
## Players
Notre jeu met en place trois personnages jouables qui sont l’archer, le mage et le démon. Les avatars des personnages ont été trouvés sur la plateforme Mixamo et sont adaptés au thème du jeu. L'archer et le mage sont les deux personnages que nous avons choisis du cahier des charges avec les actions et attaques requises. Quant au démon, il s’agit de notre personnage que nous avons implémenté avec trois attaques que nous avons créé. La première attaque est une attaque au corps à corps, la seconde est une onde de choc qui inflige des dégâts aux ennemis autour du personnage et la troisième attaque invoque une liche qui suit le personnage et qui le soigne lorsqu’il est blessé. 

Afin de mettre en place les scripts de ces personnages nous avons créé une classe IPlayer depuis laquelle les classes Demon, Mage et ArcherPlayer héritent. Dans ce script les fonctions génériques aux trois personnages y sont implémentées comme le lancement des coroutines pour chacune des actions, les améliorations basiques ainsi que le déplacement. Au niveau classes filles, ce sont des fonctions qui ont été “override” et qui permettent le lancement des animations, des effets sonores. 

Afin de permettre le déplacement du joueur en utilisant différents niveaux de hauteurs nous avons décidé de passer par le système des NavMesh Agents. Pour déterminer la future position du joueur, on vient tracer un raycast vertical à une distance pondérée par les Inputs verticaux et horizontaux. Ce système nous permet par exemple d’effectuer un dash par dessus les cases de fosses (on pondère alors les valeurs de l’input d’un facteur 5). Ce système à été créé au début du projet et même si il n’est pas idéal, il nous était difficile de le modifier dans la suite du projet.

Quant aux animations des personnages, celles-ci sont différentes d’un personnage à l’autre qu’il s’agisse des déplacements ou des attaques et proviennent elles aussi de la plateforme Mixamo. Afin de garder de la cohérence dans les déplacements et les attaques, il était important qu’un personnage puisse se mouvoir et effectuer certaines attaques comme pour l’archer lors du lancement des flèches ou le mage pour l’envoi des boules de feu. Ainsi il a été nécessaire de créer deux avatars pour différencier le haut et le bas du personnage et affilier chacune des animations au bon avatar. Une autre nécessité de la part des personnages était qu’ils n’aient plus la possibilité de bouger lors d'attaques comme la création du mur de glace pour l’archer, l’apparition de la liche pour le démon ou la pose du piège pour l’archer. Ainsi une fonction a été implémentée (SwitchMovement) afin de permettre ou non le mouvement des personnages.

Il existe un mode d’invincibilité avec l’appui sur le touche Ⅰ du clavier qui va désactiver la prise de dégâts du joueur.
## Ennemis
Il y a 4 ennemis disponibles. On a les 3 ennemis imposés, c'est-à-dire un soldat, un archer et une liche. Le dernier ennemi créé est une sorcière qui se déplace de tour en tour sur le niveau pour jeter des projectiles sur le joueur. Pour implémenter le plus facilement les ennemis, il a créé une classe IEnemy de laquelle hérite tous les ennemis ce qui permet de centraliser les fonctionnalités communes aux ennemis. Chaque ennemi implémente donc au minimum 3 fonctions qui sont, la fonction d’initialisation, la coroutine d’attaque et la coroutine de déplacement. Un choix de design a été fait sur la volonté de stopper les ennemis lorsqu’ils attaquent. 

Les interactions entre les ennemis et le reste du jeu se déroulent aux travers d’un système d'évent centralisé autour du singleton GameManager. Les ennemis se déplacent sur la carte avec un NavMesh Agent. 
## UI
Thibaut a travaillé sur l’UI : Menu Principal, Paramètres, Sélection du personnage, HUD, menu de pause et menu d’upgrade. Ajout également d’icônes manquantes comme celles des compétences ou pour les sliders de son.

Concernant les paramètres, il y a un bouton de sauvegarde activé dès qu’une modification est effectuée dans le menu. Ces modifications sont réversibles (une popup apparaît pour confirmer le choix). L’accès aux paramètres depuis la scène de jeu se fait par chargement de scènes en mode additive pour pouvoir reprendre le jeu là où il était arrêté avant la pause.

Pour les barres de vie des ennemis, chacune des barres est instanciée dans un canvas global par souci de performances. Pour les popups de dégâts, elles ont chacune leur canvas car leur apparition simultanée est finalement assez limitée.

Contrairement à ce qui est indiqué par le cahier des charges, la preview du personnage sélectionné se fait en 3D et est interactive : on peut faire tourner le personnage sur lui-même pour l’inspecter.

Pour le système d’upgrade, on peut choisir de ne pas ouvrir le menu d’amélioration dès que l’on passe un niveau. C’est un pari que le joueur doit faire : espérer améliorer de plusieurs niveaux une seule et même amélioration … encore faut-il avoir la RNG de son côté.
Enfin, toutes les barres sont des sliders car cela semblait plus facile à implémenter que des sprites de taille changeante. De même pour les cooldown, on utilise des sliders en mode radial.
## Map générée procéduralement
Sur la carte il existe une case classique où le joueur et les ennemis peuvent marcher. Une case de fosse ou l’on peut pousser un ennemi mais on ne peut pas tomber dans une fosse. Au lieu de cela on peut appuyer sur LShift et traverser la fosse d’un côté à l’autre. Cette interaction a été réalisée à l’aide d’un NavMeshLink. Une case de tour qui bloque les projectiles alliés et ennemis et sur laquelle les joueurs et ennemis peuvent monter. Seul le joueur peut descendre à l’opposé de la tour, ce qui peut lui permettre de geler un ennemi temporairement en haut d’une tour. Une case ou les ennemis vont apparaître tout au long de la partie, elles sont au nombre de 8 sur la carte. Il existe aussi différents foyers de feu dispersés sur la carte. Enfin, le centre de la carte n’est pas accessible aux ennemis. 

La carte est générée procéduralement grâce à un bruit de Perlin. On pondère ensuite nos niveaux de gris pour avoir différentes cases qui apparaissent. Il  faut ensuite dessiner les contours de la carte et placer son centre en prenant soin de supprimer les cases déjà présentes au centre de la carte. On vient ensuite calculer le NavMesh pour le déplacement.
## Système de vague 
Le système de vague est géré au sein du Singleton GameManager. A l’initialisation de la partie on vient récupérer les différentes zones de spawn des ennemis qui contiennent un script avec une fonction public permettant de lancer la récupération d’un ennemi aléatoire automatiquement via le PoolingManager. 

La prochaine vague apparaît après que tous les ennemis de la vague précédente soient morts. Le nombre d'ennemis suit la suite de Fibonnaci, et fait apparaître 2 ennemis toutes les 2 secondes.
## Système d’expérience
Le système d'expérience doit fonctionner en lien étroit avec les ennemis (puisque leur mort déclenche l’apparition des orb d’xp), le player (qui doit ramasser l’xp), le GameManager (qui doit s’occuper des paliers de niveaux) et l’UI pour afficher le niveau d'expérience et les upgrades disponibles. Pour réussir ce MicMac relativement compliqué, nous avons décidé de passer par une centralisation dans le GameManager à l’aide d’events.

Un événement est déclenché à la mort d’un ennemi pour lancer la créations des orbes d’xp avec un nombre aléatoire et des valeurs d’xp aléatoire aussi.

Un événement est déclenché au ramassage d’une orbe d’xp pour la remettre au PoolingManager et pour gérer les différents paliers de niveaux.
# Temps passé sur le projet et répartition des tâches
Samuel : 
- 100h-150h
- Ennemis
- Map
- GameManger
- Pooling Manager
- Interactions globales entre tous les composants

Thibaut : 
- 70h-80h 
- UI
- HUD
- Gestion des paramètres
- Changement de scènes

Théo :
- 60h-70h
- Joueurs
- Création d’avatar pour les animations
- Sons
# Points sur le code
## Designs Patterns
### Héritage des classes
Comme mentionné précédemment, les classes des trois différents types de personnages jouables héritent du classe mère IPlayer. Dans cette classe sont implémentées toutes les fonctions génériques aux personnages, comme le lancement des coroutines liés aux attaques, aux améliorations et le déplacement. Ces fonctions sont override pour chacune des classes filles, afin d’avoir le comportement voulu pour chaque fonction afin de lancer les bonnes animations et bons effets sonores.
L’héritage est aussi appliqué pour les classes d’ennemis avec une classe mère IEnemy.
### Utilisation de singletons
Plusieurs singletons sont utilisés pour ce projet. Le principal est le GameManager qui gère le système de vague et l'apparition des ennemis, les événements de dégâts infligés ou subis, la collection d'orbes d'expérience et la mort d'un ennemi ou du joueur. 
Cela permet notamment aux évènements du jeu de modifier l'UI grâce à cette centralisation. Un autre manager est l'AudioManager. Celui-ci permet de gérer, comme son nom l'indique, le son dans le jeu. Il permet de jouer un son ou de tous les arrêter par exemple. 
## Optimisations
### Pooling Manager
Le PoolingManager est un singleton qui permet de mettre n’importe quel objet de Unity dans une pool.
C’est un système très complet basé sur l’utilisation de classes génériques pour permettre l’initialisation de Components par exemple. 
Pour stocker les objets dans notre pool on utilise un dictionnaire de List d’Object de Unity. Les clés du dictionnaire correspondent aux noms des objets. 
	
Pour faire fonctionner l’initialisation et la modification des objets des classes génériques dérivant de l’objet primaire Object il nous a fallu surcharger les fonctions SetActive, SetParent, GetActive, SetTransform. En effet, en manipulant des classes générique on peut avoir à la fois des Components et des GameObejcts. La dernière fonction à recréer à été InstantiateDisabled pour créer des objets désactivés par défaut, ce qui empêche l'exécution du OnEnable.  
### Utilisation d’un canvas global pour les barres de vie ennemis
Les barres de vie des ennemis sont toutes instanciées en tant qu’enfant d’un canvas global. En effet, avoir un canvas pour chaque ennemi est relativement coûteux en ressource, alors qu’on pourrait utiliser un seul et même canvas et faire en sorte que la barre de vie suive la position des l’ennemi à laquelle elle est assignée, ce qui est moins coûteux. Pour une centaine de canvas dans une même scène, on peut facilement perdre 40% de performances.
# Git
Tout au long du projet nous avons synchronisé notre projet sur un Github (https://github.com/SamuelGuillemet/Projet-AMJV)  en prenant soin de respecter un GitFlow, c’est-à-dire la création de différentes branches feature que l’on prenait soin de rebase sur la branch dev avant d’effectuer les fusions (permet d’éviter les conflits de fusion). Enfin une release est apparue avec le merge de la branche dev sur main à la fin du projet. On y retrouve en détail les différentes pull requests : https://github.com/SamuelGuillemet/Projet-AMJV/releases/tag/v1
# Bugs dans le build final 
	
- Feature plus ou moins intentionnelle : les dégâts de zones de la bombe de la sorcière traversent le mur généré par le mage.

- Lorsque le menu d'améliorations est ouvert et que l'on met pause et que l'on reprend le jeu, le menu d'améliorations est toujours ouvert mais le jeu reprend en arrière-plan. Quick fix : utiliser un booléen pour vérifier si le menu d'améliorations est actif, auquel cas, garder le jeu en pause. 
