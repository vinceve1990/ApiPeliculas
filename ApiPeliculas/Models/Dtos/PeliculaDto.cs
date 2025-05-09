﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models.Dtos {
    public class PeliculaDto {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificacion { Siete, Trece, Dieciséis, Dieciocho }
        public CrearTipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
    }
}
