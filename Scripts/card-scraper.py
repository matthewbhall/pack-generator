#!/usr/bin/python

import sys
import getopt
import urllib2
import json
from bs4 import BeautifulSoup
import re
import json

def convert_mana_cost(cost):
    pieces = re.findall(r'[\dWUBRG]', cost)
    cmc = 0
    for piece in pieces:
        if piece.isdigit():
            cmc += int(piece)
        else:
            cmc += 1
    return cmc
    

def determine_if_creature(type):
    if "Creature" in type:
        return True
    return False
    
def determine_rarity(content):
    pieces = content.split(',')
    rarity = pieces[1].strip()
    return rarity

def get_card(url):
    response = urllib2.urlopen(url)
    html = response.read()
    soup = BeautifulSoup(html.decode('utf-8','replace'), 'html.parser')
    response.close()
    
    # Extract card name from title
    tag = soup.find('meta', {'name' : 'twitter:title'})
    name = tag['content']
    
    # Extract mana cost and is_creature from description
    tag = soup.find('meta', {'name' : 'twitter:description'})
    description = tag['content']
    pieces = description.split(',')
    cmc = convert_mana_cost(pieces[0])
    is_creature = determine_if_creature(pieces[1])
    
    # Extract rarity from data1
    tag = soup.find('meta', {'name' : 'twitter:data1'})
    rarity = determine_rarity(tag['content'])
    
    # Extract imgage
    tag = soup.find('meta', {'name' : 'twitter:image'})
    image = tag['content']
    index = image.find('?')
    image = image[:index]
    
    return {'name': name, 'cmc': cmc, 'is_creature': is_creature, 'rarity': rarity, 'image': image}
    

def main(argv):
    set = ''
    count = -1
    try:
        opts, args = getopt.getopt(argv,'h:sc:',['set=','count='])
    except getopt.GetoptError:
        print 'test.py -s <set abbreviation> -c <card count>'
        sys.exit(2)
    for opt, arg in opts:
        if opt == '-h':
            print '\t error: required format is "test.py -s <set abbreviation> -c <card count>"'
            sys.exit()
        elif opt in ('-s', '--set'):
            set = str.lower(arg)
        elif opt in ('-c', '--count'):
            if arg.isdigit() == False:
                print '\terror: count must be a positive whole number'
                sys.exit(2)
            count = int(arg)
        
    if set == '' or count < 0:
        print '\terror: supply a set abbreviation and card count. run test.py -h  for details.'
        sys.exit(2)
        
    
    print 'Set abbreviation is %s' % set
    print 'Card count is %d' % count
    
    data = {}
    data['cards'] = []
    for x in range(1, count + 1): #todo switch 1 to count
        url = 'https://scryfall.com/card/%s/%d' % (set, x)
        #data['cards'].append(create_card_objects(html))
        card = get_card(url)
        print card
        data['cards'].append(card)
        
    outfile = open('%s.txt' % set, 'w')
    json.dump(data, outfile)
    print 'card data written to %s.txt' % set
        

if __name__ == "__main__":
    main(sys.argv[1:]) 