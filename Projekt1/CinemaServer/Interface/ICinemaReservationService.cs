using CinemaServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Interface
{
    [ServiceContract]
    public interface ICinemaReservationService
    {
        [OperationContract]
        string Zaloguj(string imieNazwisko);

        [OperationContract]
        List<Film> GetListaFilmow();

        [OperationContract]
        string ZarezerwujMiejsce(string userId, string filmId, string seansId, int numerMiejsca);

        [OperationContract]
        string ZarezerwujWieleMiejsc(string userId, string filmId, string seansId, List<int> miejsca);

        [OperationContract]
        bool AnulujRezerwacje(string userId, string rezerwacjaId);

        [OperationContract]
        bool ModyfikujRezerwacje(string userId, string rezerwacjaId, int noweMiejsce);

        [OperationContract]
        Rezerwacja PobierzPotwierdzenieRezerwacji(string userId, string rezerwacjaId);

        [OperationContract]
        List<Rezerwacja> ListaRezerwacji(string userId);

        [OperationContract]
        Film GetFilmInfo(string filmId);

        [OperationContract]
        Seans GetSeansInfo(string seansId);

    }

}
