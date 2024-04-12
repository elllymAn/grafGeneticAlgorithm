#include "pch.h"
#include "Graph.h"

int Graph::getWeightOfCase(std::vector <Point*>& Case)
{
	int r = 0;
	std::for_each(arrEdge->begin(), arrEdge->end(),
		[&](Edge* thread)
		{
			std::vector <Point*> ::iterator it = std::find_if(Case.begin(), Case.end(),
			[&](Point* obj)
				{
					return *thread->getPair()->first->getName() == *obj->getName();
				}
	);
	std::vector <Point*> ::iterator it1 = std::find_if(Case.begin(), Case.end(),
		[&](Point* obj)
		{
			return *thread->getPair()->second->getName() == *obj->getName();
		}
	);
	r += (((abs(std::distance(it1, it))) < (abs(std::distance(it, it1)))) ? (abs(std::distance(it1, it))) : (abs(std::distance(it, it1))));
		}
	);
	return r;
}

Graph::Graph(const Graph& obj)
{
	arrPoint = new std::set <Point*>;
	arrEdge = new std::vector <Edge*>;
	std::for_each(obj.arrPoint->begin(), obj.arrPoint->end(),
		[&](Point* elem)
		{
			arrPoint->insert(new Point(*elem));
		});
	std::for_each(obj.arrEdge->begin(), obj.arrEdge->end(),
		[&](Edge* elem)
		{
			arrEdge->push_back(new Edge(*elem));
		});
}
std::multimap <int, std::vector <Point*>> Graph::GetRating()
{
	std::multimap <int, std::vector <Point*>> rating;
	for (int i = 0; i < 3; i++)
	{
		std::vector <Point*> Case;
		std::copy(arrPoint->begin(), arrPoint->end(), std::back_inserter(Case));
		std::random_shuffle(Case.begin(), Case.end());
		int r = getWeightOfCase(Case);
		//int size = rating.size();
		rating.insert(std::pair<int, std::vector <Point*>>{r, Case});
		//if (rating.size() == size) i--;
	}
	return rating;
}

std::string Graph::geneticAlgorithm()
{
	if (arrPoint->size() == 0) return "";
	std::string answer = "";
	std::multimap <int, std::vector <Point*>> rating = GetRating();
	for (int iteration = 1; iteration < 100; iteration++)
	{
		if ((iteration % (rating.begin())->second.size()) != 0)
		{
			std::vector <Point*> newCase;
			std::copy((rating.begin())->second.begin(), (rating.begin())->second.begin() + (iteration % (rating.begin())->second.size()), std::back_inserter(newCase));
			std::copy((rating.begin())->second.rbegin(), (rating.begin())->second.rend() - (iteration % (rating.begin())->second.size()), std::back_inserter(newCase));
			rating.erase(--rating.end());
			int r = getWeightOfCase(newCase);
			rating.insert(std::pair<int, std::vector <Point*>>{r, newCase});
		}
	}
	//std::cout << "---------" << std::endl;
	std::pair<std::multimap<int, std::vector <Point*>>::iterator,
		std::multimap<int, std::vector <Point*>>::iterator>
		ret = rating.equal_range(rating.begin()->first);

	std::vector <std::string> any_graf;
	std::set <std::string> buffer;
	for (; ret.first != ret.second; ret.first++)
	{
		std::for_each((ret.first)->second.begin(), (ret.first)->second.end(),
			[&answer](Point* obj)
			{
				answer += *obj->getName() + ",";
			});
		answer = answer.substr(0, answer.length() - 1) + ";";
		std::for_each(arrEdge->begin(), arrEdge->end(),
			[&answer](Edge* obj)
			{
				answer += *obj->getPair()->first->getName() + "-" + *obj->getPair()->second->getName() + ",";
			});
		buffer.insert(answer);
		answer = "";
	}
	std::set_union(any_graf.cbegin(), any_graf.cend(),
		buffer.cbegin(), buffer.cend(),
		std::back_inserter(any_graf)
	);
	for (std::vector <std::string> ::iterator it = any_graf.begin(); it != any_graf.end(); ++it) answer += (*it) + "/";
	//std::cout << "---------" << std::endl;
	return answer;
}

Graph::Graph(std::string data)
{
	arrPoint = new std::set <Point*>;
	arrEdge = new std::vector <Edge*>;
	std::string points = data.substr(0, data.find(";"));
	while (points.find(",") != std::string::npos)
	{
		//std::cout << points.substr(0, points.find(",")) << std::endl;
		arrPoint->insert(new Point(points.substr(0, points.find(","))));
		points = points.substr(points.find(",") + 1, points.length());
	}
	arrPoint->insert(new Point(points));
	points = data.substr(data.find(";") + 1);
	while (points.find(",") != std::string::npos)
	{
		Point* p1 = nullptr;
		Point* p2 = nullptr;
		std::string edge = points.substr(0, points.find(","));
		std::for_each(arrPoint->begin(), arrPoint->end(),
			[&edge, &p1, &p2](Point* obj)
			{
				//std::cout << edge.substr(0, edge.find("-")) << std::endl;
				//std::cout << edge.substr(edge.find("-") + 1, edge.length()) << std::endl;
				if (*obj->getName() == edge.substr(0, edge.find("-")))
					p1 = obj;
				if (*obj->getName() == edge.substr(edge.find("-") + 1, edge.length()))
					p2 = obj;
			});
		arrEdge->push_back(new Edge(p1, p2));
		//delete p1; delete p2;
		points = points.substr(points.find(",") + 1, points.length());
	}
}
Graph::~Graph()
{
	std::for_each(arrPoint->begin(), arrPoint->end(),
		[](Point* obj)
		{
			delete obj;
		});
	std::for_each(arrEdge->begin(), arrEdge->end(),
		[](Edge* obj)
		{
			delete obj;
		});
	delete arrEdge;
	delete arrPoint;
}