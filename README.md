# Faust
An RPG based on the play "Faust" by Johann Wolfgang von Goethe

As of right now, only 1 level is finished. Level 1 is based off of Scene V in Faust Part One. 

//
[Dialogue System]

The dialogue system utilized in this game was built from scratch. It emphazies modularity and code reusability in order to streamline the process of creating multi-branching dialogue. The branching of each NPC's dialogue is defined by a block of code called a "quest package". Each individual quest package share the same basic structure. The actual dialogue itself is not hard coded into the game itself. Rather, they are read from JSON files to enable convenient editing: further following the principle of modularity.
