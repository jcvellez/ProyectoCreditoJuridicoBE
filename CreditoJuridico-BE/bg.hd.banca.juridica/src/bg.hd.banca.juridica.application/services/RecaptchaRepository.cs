﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.domain.entities.recaptcha;
using bg.hd.banca.juridica.application.models.exceptions;

namespace bg.hd.banca.juridica.application.services
{
    public class RecaptchaRepository: IRecaptchaRepository
    {
        private readonly IRecaptchaRestRepository _recaptchaRestRepository;
        public RecaptchaRepository(IRecaptchaRestRepository recaptchaRestRepository)
        {
            this._recaptchaRestRepository = recaptchaRestRepository;
        }

        public async Task<RecaptchaResponse> validarRecaptcha(RecaptchaRequest request)
        {
            
            #region validaCampoToken
            if (request.token.Trim() == "" || request.token is null)
            {
                throw new RecaptchaException("Token es requerido", "Token es requerido", 2);
            }
            #endregion

            return await _recaptchaRestRepository.validarRecaptcha(request);
        }
    }
}
