#!/usr/bin/python

import sys
import getopt
import urllib2
import json
from bs4 import BeautifulSoup
import re
import json

colors = {'W': 'White', 'U': 'Blue', 'B': 'Black', 'R': 'Red', 'G': 'Green'}
colorless = "Colorless"
multicolored = "Multicolored"

def extract_cmc_and_color(cost):
    pieces = re.findall(r'[\dWUBRG]', cost)
    cmc = 0
    color = colorless
    for piece in pieces:
        if piece.isdigit():
            cmc += int(piece)
        else:
            if color == colorless:
                color = colors[piece]
            elif color != colors[piece]:
                color = multicolored
            cmc += 1
    return cmc, color
    

def extract_if_creature(type):
    if "Creature" in type:
        return True
    return False
    
def extract_rarity(content):
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
    
    # Extract mana cost, is_creature, and is_basic from description
    tag = soup.find('meta', {'name' : 'twitter:description'})
    description = tag['content']
    pieces = description.split(',')
    if "Basic Land" in pieces[0]:
        cmc = 0
        color = colorless
        is_creature = False
        is_basic = True
    else:
        cmc, color = extract_cmc_and_color(pieces[0])
        is_creature = extract_if_creature(pieces[1])
        is_basic = False
    
    # Extract rarity from data1
    tag = soup.find('meta', {'name' : 'twitter:data1'})
    rarity = extract_rarity(tag['content'])
    
    # Extract imgage
    tag = soup.find('meta', {'name' : 'twitter:image'})
    image = tag['content']
    index = image.find('?')
    image = image[:index]
    
    return {'Name': name, 'ConvertedManaCost': cmc, 'Color': color, 'IsCreature': is_creature, 'IsBasic': is_basic, 'Rarity': rarity, 'Image': image}
    

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
    
    data = []
    for x in range(1, count + 1): #todo switch 1 to count
        url = 'https://scryfall.com/card/%s/%d' % (set, x)
        card = get_card(url)
        print card
        data.append(card)
        
    outfile = open('%s.txt' % set, 'w')
    json.dump(data, outfile)
    print 'card data written to %s.txt' % set
        

if __name__ == "__main__":
    main(sys.argv[1:]) 