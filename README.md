# DofLog

[![download](https://img.shields.io/github/downloads/oxypomme/DofLog/total?style=for-the-badge)](https://shields.io)
[![version](https://img.shields.io/github/v/release/oxypomme/DofLog?label=Version&style=for-the-badge)](https://shields.io)
<a href="https://forthebadge.com/"><img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg" alt="forthebadge" height="28"/></a>

> DofLog est un outil de connexion automatique pour Dofus développé en C#.

<img src="DofLog/icon.ico" alt="drawing" width="124"/>

## Pourquoi faire

Dofus est un jeu où le multi compte est, s'il n'est pas encouragé, assez optimisé.

Malheureusement, connecter plusieurs comptes est long et fastidieux...\
Mais pour cela il existe des outils, notamment le [nAiO](https://naio.fr/), de ZDS, qui connecte à notre place tout nos comptes. Mais il marche assez mal avec l'Ankama Launcher. Ce dernier possède aussi une fonctionnalité pour le multi compte, mais elle est trop incomplète...

C'est pourquoi j'ai développé `DofLog`, un outil pour se connecter automatiquement à Dofus en passant par l'Ankama Launcher !

![Header](https://i.imgur.com/uASiRSs.png)<br/>
<font size="2">*DofLog dans sa version 2.0.0*</font>

## Installation

### Avant de commencer

**<font color=red>
Ne donnez à personne le fichier `config.ser` et ce peu importe les conditions !
</font>**

Ce fichiers renferme vos paramètres _mais aussi les identifiants de vos comptes_. L'équipe de développement n'est en aucun cas responsable si vous vous faites pirater votre compte après avoir confié votre fichier `config.ser` à quiconque.

Si un problème survient, n'hésitez pas à contacter l'équipe de développement ou de faire une `issue` sur GitHub. L'équipe de développement ne vous demandera **JAMAIS** votre fichier de configuration.

_DofLog n’a pas été autorisé par la société Ankama_ et ne respecte (probablement) pas les [CGU](https://www.dofus.com/fr/cgu) *(c.f [le cas nAiO](https://www.dofus.com/fr/forum/1003-divers/2299315-pouvons-utilise-naio-2?page=1#entry12456707), l'inspiration principale de DofLog)* et un risque de ban est présent. _L’équipe de développement ne peut être tenue responsable en cas de ban_. Cependant, DofLog ne modifie pas le jeu ou ses fichiers, il simule un clavier et une souris (en intéragissant avec Windows) et prend des captures d'écran d'un pixel afin d'obtenir la couleur du dit pixel. Par conséquent, DofLog est difficilement détéctable *(autant que nAiO en fait)*. L'objectif principal de DofLog est d'être un gestionnaire de mot de passes pour des comptes Dofus.

Si Ankama estime qu’une des fonctionnalités de DofLog va à l’encontre de ce qu’ils souhaitent, l'équipe de développement de DofLog retira cette fonctionnalité.

### Installation

```md
Installer la dernière version de l'Ankama Launcher.
Connectez vous avec un compte (n'importe lequel, on va régler 2~3 trucs).
Installez Dofus.
Pendant ce temps, cliquez sur la petite roue dentée (Paramètres) et
    Vérifiez que le Launcher ne se réduit pas dans la zone de notification après le lancement d'un jeu.
Déconnectez vous.
```

Téléchargez la dernière version dans l'onglet [releases](https://github.com/oxypomme/DofLog/releases).

OS X & Linux:

```md
Non supporté
```

Windows:

```md
Téléchargez la dernière version.
Installez.
Jouez !
```

## Utilisation

![Create](https://i.imgur.com/B8zNWjk.gif)

Création rapide et simple d'un compte

![Edit](https://i.imgur.com/qK9EKQF.gif)

Possibilité d'édition des comptes

![Delete](https://i.imgur.com/LqDixtt.gif)

Vous ne jouez plus un compte ? Vous pouvez le supprimer simplement.

![Groups](https://i.imgur.com/BVEjK4S.gif)

Vous connectez souvent les mêmes comptes ? Créez un groupe !

![Order](https://i.imgur.com/GNg7I2q.gif)

Le changement de place dans la liste est possible.

![Reduce](https://i.imgur.com/NPy6dQq.png)

Réduction dans la zone des notifications quand vous le voulez.

![Order2](https://i.imgur.com/9qntTXO.gif)

L'ordre est important ! DofLog sauvegarde dans quel ordre vous avez coché vos compte pour les connecter dans l'ordre.

![Discord](https://i.imgur.com/U67WS0N.png)

Vous aimez faire savoir à tout le monde que vous jouer à Dofus ? DofLog le fait pour vous.

![Retro](https://i.imgur.com/zheKcWu.gif)

DofLog vous connecte à Dofus... Que ce soit la version 2 ou la version retro.

## Développé avec

- [Octokit.net](https://github.com/octokit/octokit.net)
- [Discord RPC C#](https://github.com/Lachee/discord-rpc-csharp)
- [Json.NET](https://www.newtonsoft.com/json)

## Versioning

J'utilise [SemVer](http://semver.org/) pour le versioning. Pour la liste complète des versions, visitez les [tags de ce repo](https://github.com/oxypomme/DofLog/tags).

## Historique des Versions

- 2.0.2
  - Diverses optimisations
  - Groupes de comptes :
    - Clic droit sur le bouton connecter pour accéder au groupes de compte
  - Barre de défilement colorée en fonction du mode (Dofus Rétro ou Dofus 2)
  - Possibilité de télécharger directement la nouvelle mise à jour
  - Cryptage des comptes
- 2.0.1
  - Raccourcis claviers :
  - Fenêtre principale :
    - `Ctrl + N` > Créer un nouveau compte
    - `Ctrl + E`  > Édite le compte sélectionné
    - `Suppr`    > Supprime le compte sélectionné
    - `Ctrl + Flèche` > Bouge le compte sélectionné dans la liste
  - Création / Édition de compte :
    - `Entrée` > Valider
    - `Echap` > Annuler
  - Lors de la création de compte, le surnom est sélectionné par défaut
  - Lors d'un mouvement du compte dans la liste, le compte est re-sélectionné
  - La taille de la fenêtre est maintenant sauvegardée
  - Possibilité de décocher tout les comptes cochés
- 2.0.0
  - Sortie officielle de DofLog !
    - Possibilité d'ajouter, retirer et éditer les comptes
    - Raccourci vers l'[Organizer](http://update.naio.fr/v2/Organizer/1.4/Organizer.zip) de ZDS
    - Intégration discord
    - Compatible Dofus 2 / Dofus retro

## Auteurs

- [**OxyTom**](https://github.com/oxypomme) - [@OxyTom](https://twitter.com/OxyT0m8)

Lisez la liste des [contributeurs](https://github.com/oxypomme/DofLog/contributors) qui ont participé à ce projet.

[![license](https://img.shields.io/github/license/oxypomme/DofLog?style=for-the-badge)](https://github.com/oxypomme/DofLog/blob/master/LICENSE)

[Dofus](https://dofus.com/fr) est un MMORPG édité par [Ankama](https://www.ankama.com/fr).

DofLog est un outil non-officiel sans aucun lien avec Ankama.

Certaines illustrations sont la propriété d'Ankama Studio et de Dofus - Tous droits réservés

## Contribute

1. Fork it (<https://github.com/oxypomme/DofLog/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

## Bug Report

Avant de poster le moindre bug report, merci de respecter ces quelques règles :

> Le formatage des problèmes (Issues) est le suivant :
> - :question: en cas de suggestion
> - :warning: en cas de problème qui n'est pas critique
> - :red_circle: en cas de problème critique
> - Mettre le label correspondant : `bug`, `invalid` ou `suggest`
> - Une description du problème est requise
> - *Si possible, une image du problème (un gif est encore plus explicit)*

## Remerciements

- Le [nAiO](https://naio.fr/), de ZDS, pour son concept te pour l'Organizer.
- [Dofus](https://dofus.com/fr), développé par [Ankama](https://www.ankama.com/fr), pour continuer à nous amusez après tant d'années.
- [shields.io](https://shields.io) and [ForTheBadge](https://forthebadge.com) for those quality badges.
