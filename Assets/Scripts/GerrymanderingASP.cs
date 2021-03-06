using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GerrymanderingASP
{
   public static string GetASP()
   {
        return areaSetup + districts + borderRules + population /*+ neighbours*/;
   }

   private static string areaSetup = @"

        #const max_width = 10.
        #const max_height = 10.
        #const max_district = 5.

        width(1..max_width).
        height(1..max_height).
        district_types(1..max_district).
        %population(min_pop..max_pop).
        population(80;120).

        tile_type(filled; empty).
  
        1{tile(XX,YY, Type): tile_type(Type)}1 :- width(XX), height(YY).

     ";

    private static string districts = @"
    %% Create districts
        %1{district(XX,YY, Type): district_types(Type)}1 :- tile(XX,YY,_).

        :- tile(XX,YY,_),  not district(XX,YY,_).

        1{district(XX,YY,Type,Type): tile(XX,YY,_)}1 :- district_types(Type).
        :- district(XX,YY,Type), district(XX,YY,Type2), Type != Type2.

        district(XX,YY,Type) :- district(XX,YY,Type,Type).
        {district(XX,YY,Type)}1 :- tile(XX,YY,_), district(XX-1, YY,Type).
        {district(XX,YY,Type)}1 :- tile(XX,YY,_), district(XX+1, YY,Type).
        {district(XX,YY,Type)}1 :- tile(XX,YY,_), district(XX, YY-1,Type).
        {district(XX,YY,Type)}1 :- tile(XX,YY,_), district(XX, YY+1,Type).

        neighbor(District, up, Neighbor) :- district(XX,YY, District), district(XX,YY+1, Neighbor), District != Neighbor.
        neighbor(District, down, Neighbor) :- district(XX,YY, District), district(XX,YY-1, Neighbor), District != Neighbor.
        neighbor(District, left, Neighbor) :- district(XX,YY, District), district(XX-1,YY, Neighbor), District != Neighbor.
        neighbor(District, right, Neighbor) :- district(XX,YY, District), district(XX+1,YY, Neighbor), District != Neighbor.

        %use neighboors to restrict districts from being inclosed withing another district
   

        island_types(orange;red;purple;green;blue).
        1{island(Type,XX,YY): island_types(Type)}1 :- district(XX,YY,_).

        combo(-1;0;1).
        %two neighboring tiles with same island type but in different districts is incorrect
        %:- island(Type, XX, YY), island(Type, XX+I, YY+J), district(XX,YY, District), district(XX+I,YY+J, District2), District != District2, combo(I), combo(J).
        :- island(Type, XX, YY), island(Type, XX+1, YY+0), district(XX,YY, District), district(XX+1,YY+0, District2), District != District2.
        :- island(Type, XX, YY), island(Type, XX-1, YY+0), district(XX,YY, District), district(XX-1,YY+0, District2), District != District2.
        :- island(Type, XX, YY), island(Type, XX+0, YY+1), district(XX,YY, District), district(XX+0,YY+1, District2), District != District2.
        :- island(Type, XX, YY), island(Type, XX+0, YY-1), district(XX,YY, District), district(XX+0,YY-1, District2), District != District2.
    

        %if diagonal tiles are in the same island they must have a neighbor in common in the same island
        combo2(-1;1).
        :- island(Island,XX,YY), island(Island,XX-1, YY+1), Count = {island(Island,XX-1,YY); island(Island,XX,YY+1)}, Count == 0.
        :- island(Island,XX,YY), island(Island,XX+1, YY+1), Count = {island(Island,XX+1,YY); island(Island,XX,YY+1)}, Count == 0.

        %two neighboring tiles with same district but are on differnt islands is incorrect
        :- island(Type, XX, YY), island(Type2, XX+I, YY+J), district(XX,YY, District), district(XX+I,YY+J, District), Type != Type2, combo(I), combo(J).
    ";

    private static string borderRules = @"
    %% shape and border control  
    
        %:- Count = {district(_,_,Type)}, district_types(Type), Count == 0.

        spikey(XX,YY,District) :- {district(XX+1,YY,District);
                                  district(XX-1,YY,District);
                                  district(XX,YY+1,District);
                                  district(XX,YY-1,District);
                                  district(XX+1,YY+1,District);
                                  district(XX+1,YY-1,District);
                                  district(XX-1,YY+1,District);
                                  district(XX+1,YY+1,District)} <= 2, 
                                  district(XX,YY,District).
        spikey_count(District, Count) :- Count = {spikey(XX,YY,District)}, district_types(District).
        :- spikey_count(District, Count), district_types(District), Count > 1.
    ";

    private static string population = @"
    %% population and regions
        1{population(XX, YY, Population): population(Population)}1 :- tile(XX, YY, _).
    
        region(XX,YY,rural) :- tile(XX,YY,_),  population(XX,YY,Population), Population < 101.
        region(XX,YY,city) :- tile(XX,YY,_),  population(XX,YY,Population), Population > 100.

        %population(XX,YY,Population, District) :- population(XX,YY,Population), district(XX,YY,District).
        %district_pop(District, Pop) :- Pop = #sum{Count, XX,YY : population(XX,YY,Count,District)}, district_types(District).
    
        %district_pop(District, Pop) :- Pop = #sum{Count, XX, YY : population(XX,YY,Count), district(XX,YY,District)}, district_types(District).
        %:- district_pop(D1, P1), district_pop(D2, P2), P1 > P2 + 1000.

        district_pop(District, Area) :- Area = {district(XX,YY,District)}, district_types(District).
        %:- district_pop(District, Area), Area == 1.
        :- district_pop(D1, A1), district_pop(D2, A2), A1 > A2 + 10.

        %district_pop(District, Rural, City) :- 
        %    Rural = {district(XX,YY,District): region(XX,YY,rural)}, 
        %    City = {district(XX,YY,District): region(XX,YY,city)},
        %    district_types(District).
        %:- district_pop(D1, R1, C1), district_pop(D2, R2, C2), R1 + C1*100 > R2 + C2*100 + 100.

    ";

    private static string neighbours = @"
    %% neighbors
        neighbour(District, up, Neighbour) :- district(XX,YY,District), district(XX, YY+1, Neighbour), District != Neighbour.
        neighbour(District, down, Neighbour) :- district(XX,YY,District), district(XX, YY-1, Neighbour), District != Neighbour.
        neighbour(District, left, Neighbour) :- district(XX,YY,District), district(XX-1, YY, Neighbour), District != Neighbour.
        neighbour(District, right, Neighbour) :- district(XX,YY,District), district(XX+1, YY, Neighbour), District != Neighbour.

        neighbour(District, Neighbours) :- district_types(District), district_types(Neighbour), 
                    Neighbours = {
                    neighbour(District, up, Neighbour);
                    neighbour(District, down, Neighbour);
                    neighbour(District, left, Neighbour);
                    neighbour(District, right, Neighbour)}.

        :- neighbour(District, Neighbours), Neighbours > 2.

        %#show district_pop/2.
    ";
}
