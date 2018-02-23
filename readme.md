Development workflow: #

all features/issues are done in a seprate branch branched off of development branch
name your branches the same as jira tickets [ Number index only is sufficient; For example E2.2 ]
always branch off of development branch
no direct commits in development/master branch
after you push your branch, request a code review and your branch will be merged by TL
if you need to merge the code in development branch asap so you can continue working, ping the TL immediately

Standards and pointers #

classes should not be longer than 300 LOC
methods should not be longer than 30 LOC
all architecture needs to be fully decoupled
Stick to DRY principles by any means
commit your code as often as possible [please, please keep commit diffs as small as possbile]
push your code at least once a day
if you get stuck for >2h by not being able to solve a problem, ping in development group chat for help/support/brainstorm => do not waste time!
do not add huge libraries as a core dependency to solve a problem that can be solved in one custom class
base your implemention on design patterns => do not reinvent the wheel poorly
HAVE FUN :)