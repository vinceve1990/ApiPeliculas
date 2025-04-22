﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models {
    public class Pelicula {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciséis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Relacion Tabla Categoria
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
