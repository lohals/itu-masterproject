﻿grammar AendringDefinitionGrammar;

/*
 * Parser Rules
 */
 /*root definition */
aendringDefinition
	: phrase ('.'|':')? EOF
	;

/* possible AD phrase types */
phrase
	:parentTargetChangedExp
	|parentTargetRemovedExp
	
	
	|insertLastExp
	|insertAfterExp
	|insertAfterChainExp
	|insertBeforeExp

	|removeExp

	|globalReplaceExp
	|replaceExp
	
	

	|manualExp
	;

/* phrases */
globalReplaceExp
	:'Overalt i loven' replaceAktionExp multiQuotedTextChangeExp
	;
parentTargetChangedExp
	: 'Paragraffen affattes således'
	;
parentTargetRemovedExp
	: 'Paragraffen udgår'
	;

insertAfterChainExp
	: 'I' elementChainExp insertAfterAktionExp (quotedTextChangeExp|(lastElementExp asnewElementExp?)) 
	;
insertAfterExp:
	'Efter' (elementExp) insertAfterAktionExp
	;										  
	
insertLastExp
	: 'Som' lastElementExp 'indsættes'
	| 'I' elementChainExp ('indsættes som'|', indsættes som') lastElementExp ('og' INT)?
	;
insertBeforeExp
	: 'Før' elementChainExp 'indsættes som nyt nummer'
	|'I' elementChainExp (', indsættes før'|'indsættes før') (elementOrOpregningExp ('som nye stykker'|'som nye numre'))
	;

removeExp
	: rootedMultiElementExp removeAktionExp
	|elementChainExp removeAktionExp
	| 'I' elementChainExp removeAktionExp QUOTEDTEXT
	;

replaceExp
	:rootedBecomesExp replaceAktionExp
	|rootedMultiElementExp replaceAktionExp
	|elementChainExp replaceAktionExp
	| 'I' elementChainExp becomesExp replaceAktionExp quotedTextChangeExp
	| 'I' (elementChainExp|multiElementExp) replaceAktionExp multiQuotedTextChangeExp//§ 98 e, stk. 4, 2. pkt., der bliver stk. 5, 2. pkt., affattes således:
	
	;

rootedBecomesExp
	:rootElementExp ', ' ignoreableElementChainExp becomesChainExp	
	;

rootedMultiElementExp
	:rootElementExp ', ' rangedMultiElementExp
	|rootElementExp ', ' multiElementExp //§ 9 h, stk. 1 og 2, affattes således:
	;
rootElementExp
	:elementExp
	;
manualExp
	:'I'? elementChainExp ((removeAktionExp QUOTEDTEXT)|('ophæves'|', ophæves')) manuelTextBitExp+
	|elementChainExp ((', '|'og') ignoreableElementChainExp)+ manuelTextBitExp+
	;
manuelTextBitExp:
	', ' ignoreableElementChainExp removeAktionExp
	|(', og i' ignoreableElementChainExp removeAktionExp QUOTEDTEXT)
	|(', og' ignoreableElementChainExp (', 'ignoreableElementChainExp)? removeAktionExp) ', og i stedet indsættes'?
	|(', og i' ignoreableElementChainExp 'indsættes efter' quotedTextChangeExp)
;


/*specialized meta element categories*/
elementOrOpregningExp
	:elementExp|opregningExp
	;
ignoreableElementChainExp
	:elementChainExp|multiElementExp
	;
multiElementExp
	:INT '. og' elementExp
	|elementExp 'og' INT 
	|elementChainExp ('og'|', og') elementChainExp
	;

rangedMultiElementExp
	:elementExp '-' INT
	;
lastElementExp
	: elementExp|opregningExp
	;

becomesExp
	:', der bliver ' elementExp
	;

becomesChainExp
	:', der bliver ' elementChainExp
	;
/* meta element categories */
elementChainExp
	: (elementExp) (', ' elementExp)* (', ' opregningExp)*
	|elementExp 'og' elementExp
	;

elementExp 
	: paragrafExp|stkExp|pktExp
	;
opregningExp
	:nummerOpregningExp|litraOpregningExp
	;

multiQuotedTextChangeExp
	:quotedTextChangeExp (', og' quotedTextChangeExp)*
	 ;
quotedTextChangeExp
	: QUOTEDTEXT 'til:' QUOTEDTEXT
	| QUOTEDTEXT ':' QUOTEDTEXT
	| QUOTEDTEXT 'ændres til:'QUOTEDTEXT
	| QUOTEDTEXT 'ændres til'QUOTEDTEXT
	;

/*Textual context expressions*/

removeAktionExp
	:', udgår'|'udgår'|',udgår'|', ophæves'|'ophæves';
insertAfterAktionExp
	:', indsættes efter'
	 |'indsættes før overskriften før' elementExp 
	 |'indsættes efter'
	 |'indsættes som nyt stykke'
	 |'indsættes som nye stykker'
	 |'indsættes som ny paragraf'
	 |'indsættes'

	;
replaceAktionExp
	:', ændres'|'ændres'|', affattes således'|'affattes således'|', ophæves, og i stedet indsættes'|'ophæves, og i stedet indsættes'
	;
asnewElementExp:'som nyt nummer'|'som nyt stykke'|'som nye numre';
/*concret element expressions*/

nummerOpregningExp
	: 'nr. ' INT
	;

litraOpregningExp
	:'litra ' LETTER+;
paragrafExp 
	: '§ ' INT LETTER?
	;
flereStkExp
	:
	'Stk. ' INT (('og'|'-'|',')INT)+
	;
stkExp 
	: ('Stk. '|'stk. ') INT
	;
pktExp
	: INT ('. pkt'|'. pkt.')
	;

/*
 * Lexer Rules
 */


INT : [0-9]+; 
LETTER : [a-z]|[A-Z]; 
QUOTEDTEXT : {_input.La(-1) == '»';}? FREETEXT {_input.La(1) == '«';}?;

fragment FREETEXT: .*?;
//SPACE:' ';
//ANYCHAR: .;

WS
	:	' ' -> channel(HIDDEN)
	;