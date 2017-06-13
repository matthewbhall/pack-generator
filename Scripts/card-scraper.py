#!/usr/bin/python

import sys
import getopt
import urllib2
import json
from bs4 import BeautifulSoup

def get_card_object(url):
    response = urllib2.urlopen(url)
    html = response.read()
    soup = BeautifulSoup(html.decode('utf-8','replace'), 'html.parser')
    response.close()
    
    title = soup.find('meta', {'name' : 'twitter:title'})
    print 'title %s' % title['content']
    

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
    for x in range(1, count): #todo switch 1 to count
        url = 'https://scryfall.com/card/%s/%d' % (set, x)
        #data['cards'].append(create_card_objects(html))
        get_card_object(url)
        print data
        

if __name__ == "__main__":
    main(sys.argv[1:]) 