grammar AendringDefinitionGrammar;

/*
 * Parser Rules
 */
 /*root definition */
aendringDefinition
	: phrase ('.'|':') EOF
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
	|replaceExp
	|manualExp
	;

/* phrases */
parentTargetChangedExp
	: 'Paragraffen affattes således'
	;
parentTargetRemovedExp
	: 'Paragraffen udgår'
	;
insertAfterChainExp
	:'I' elementChainExp ', '? 'indsættes efter' (quotedTextChangeExp|(lastElementExp ('som nyt stykke'|'som nyt nummer')))
	;
insertAfterExp:
    'Efter' (aendringsNummerExp|elementExp) ('indsættes som nyt stykke'|'indsættes som nye stykker'|'indsættes som nyt nummer'|'indsættes som nye numre'|'indsættes som ny paragraf'|'indsættes')
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
	: elementChainExp (', udgår'|'udgår'|', ophæves'|'ophæves')
	| 'I' elementChainExp 'udgår' QUOTEDTEXT
	;
replaceExp
	: elementChainExp  (', affattes således'|'affattes således'|', ophæves, og i stedet indsættes'|'ophæves, og i stedet indsættes')
	| 'I' elementChainExp (', ændres'|'ændres') quotedTextChangeExp
	| ('I det under'|'I den under'|'Den under'|'Det under') 
	aendringsNummerExp 'foreslåede' ('affattelse af')? elementChainExp 
	(', affattes således' |(', ændres'|'ændres') ('i' elementChainExp)? quotedTextChangeExp)
	;

manualExp
	:'I'? elementChainExp (((', udgår'|'udgår') QUOTEDTEXT)|('ophæves'|', ophæves')) manuelTextBitExp+
	|elementChainExp ((', '|'og') ignoreableElementChainExp)+ manuelTextBitExp+
	;
manuelTextBitExp:
    ', ' ignoreableElementChainExp ('ophæves'|', ophæves')
	|(', og i' ignoreableElementChainExp (', udgår'|'udgår') QUOTEDTEXT)
	|(', og' ignoreableElementChainExp (', 'ignoreableElementChainExp)? ('ophæves'|', ophæves')) ', og i stedet indsættes'?
	|(', og i' ignoreableElementChainExp ('indsættes efter'|'indsættes efter') quotedTextChangeExp)
;


/*specialized meta element categories*/
elementOrOpregningExp
    :elementExp|opregningExp
	;
ignoreableElementChainExp
	:elementChainExp|multiElementExp
	;
multiElementExp:
    INT '. og' elementExp
	|elementChainExp ('og'|', og') elementChainExp
    ;
lastElementExp
	: elementExp|opregningExp
    ;


/* meta element categories */
elementChainExp
    : (aendringsNummerExp|elementExp) (', ' elementExp)* (', ' opregningExp)*
	|elementExp 'og' elementExp
	;

elementExp 
	: paragrafExp|stkExp|pktExp
	;
opregningExp
    :nummerOpregningExp
    ;
quotedTextChangeExp
	: QUOTEDTEXT 'til:' QUOTEDTEXT
	| QUOTEDTEXT ':' QUOTEDTEXT 
	;


/*concret element expressions*/
aendringsNummerExp
	: ('nr. '|'Nr. ') INT
	;
nummerOpregningExp
	: 'nr. ' INT
	;
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
//comma : ','|', '; 


/*
 * Lexer Rules
 */
INT : [0-9]+; 
LETTER : [a-z]|[A-Z]; 
QUOTEDTEXT : '»' FREETEXT '«';
fragment FREETEXT: .*?;
//SPACE:' ';
//ANYCHAR: .;

//WS	:	(' ' )-> channel(HIDDEN)
//	//| '\r' | '\n'
//	;